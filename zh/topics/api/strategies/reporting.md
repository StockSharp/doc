# 战略报告

## 概览

StockSharp 提供了一个用于策略交易结果的报表生成系统。该系统由两个关键组成部分构建：

- **`IReportSource`** -- 描述报告数据源的接口（策略参数、订单、交易、持仓、统计）。
- **`IReportGenerator`** -- 一种支持多种格式（CSV、JSON、XML、Excel）的报表生成器接口。

`Strategy` 类实现了 `IReportSource` 接口，因此策略可以直接传递给报表生成器。

## IReportSource 接口

`IReportSource` 接口提供生成报告所需的所有数据：

| 属性 | 类型 | 描述 |
|----------|------|-------------|
| `Name` | `string` | 策略名称 |
| `TotalWorkingTime` | `TimeSpan` | 总工作时间 |
| `Commission` | `decimal?` | 总佣金 |
| `Position` | `decimal` | 当前持仓 |
| `PnL` | `decimal` | 总利润/亏损 |
| `Slippage` | `decimal?` | 总滑点 |
| `Latency` | `TimeSpan?` | 总延迟 |
| `Parameters` | `IEnumerable<(string, object)>` | 策略参数 |
| `StatisticParameters` | `IEnumerable<(string, object)>` | 统计参数 |
| `Orders` | `IEnumerable<ReportOrder>` | 订单 |
| `OwnTrades` | `IEnumerable<ReportTrade>` | 自有交易 |
| `Positions` | `IEnumerable<ReportPosition>` | 位置回程 |

在读取数据之前，会调用 `Prepare()` 方法来同步源的内部状态。

## ReportSource 类

`ReportSource` 是 `IReportSource` 的独立实现，不依赖于 `Strategy` 类。它允许手动构建报告的数据源：

```csharp
var source = new ReportSource();
source.Name = "My strategy";
source.PnL = 15000m;
source.TotalWorkingTime = TimeSpan.FromHours(8);

source.AddParameter("Timeframe", "5 minutes");
source.AddStatisticParameter("Sharpe Ratio", 1.85);

source.AddOrder(new ReportOrder(
    Id: 123,
    TransactionId: 456,
    SecurityId: securityId,
    Side: Sides.Buy,
    Time: DateTime.UtcNow,
    Price: 100m,
    State: OrderStates.Done,
    Balance: 0,
    Volume: 10,
    Type: OrderTypes.Limit
));
```

### 数据汇总

在大量订单和交易的情况下，`ReportSource` 自动聚合数据以减少报告大小：

```csharp
// 自动聚合阈值（默认值为 10000）
source.MaxOrdersBeforeAggregation = 5000;
source.MaxTradesBeforeAggregation = 5000;

// 分组间隔（默认 1 小时）
source.AggregationInterval = TimeSpan.FromMinutes(30);

// 手动聚合
source.AggregateOrders(TimeSpan.FromHours(1));
source.AggregateTrades(TimeSpan.FromHours(1));
```

在汇总过程中，订单和交易按时间间隔、交易品种和方向进行分组。交易量进行求和，价格按加权平均计算。

## 持仓生命周期追踪器

`PositionLifecycleTracker` 跟踪持仓的生命周期并生成往返记录——持仓开仓和平仓的记录。当发生以下情况时，会记录一次往返：


- 一个持仓已完全平仓（价值变为零）
- 位置反转发生（符号变化）

在 `Strategy` 类中，跟踪器是自动集成的：完成的往返行程通过 `RoundTripClosed` 事件被添加到 `ReportSource`。

```csharp
var tracker = new PositionLifecycleTracker();

// 往返交易关闭事件
tracker.RoundTripClosed += roundTrip =>
{
    Console.WriteLine($"Position closed: {roundTrip.SecurityId}, " +
        $"Open: {roundTrip.OpenTime} at {roundTrip.OpenPrice}, " +
        $"Close: {roundTrip.CloseTime} at {roundTrip.ClosePrice}, " +
        $"Max volume: {roundTrip.MaxPosition}");
};

// 处理持仓更新
tracker.ProcessPosition(position);

// 访问往返交易历史
IReadOnlyList<ReportPosition> history = tracker.History;
```

## 报表生成器

以下发电机可用：

| 生成器 | 格式 | 描述 |
|-----------|--------|-------------|
| `CsvReportGenerator` | CSV | 带分隔符的文本格式 |
| `JsonReportGenerator` | JSON | 结构化 JSON |
| `XmlReportGenerator` | XML | XML 格式 |
| `ExcelReportGenerator` | Excel | Excel 格式（需要 `IExcelWorkerProvider`） |

所有生成器都继承自 `BaseReportGenerator`，并支持包含部分的配置：

```csharp
var generator = new CsvReportGenerator();

// 配置报表章节
generator.IncludeOrders = true;
generator.IncludeTrades = true;
generator.IncludePositions = true;
generator.Encoding = Encoding.UTF8;
```

## 从策略生成报告

由于 `Strategy` 实现了 `IReportSource`，可以直接生成报告：

```csharp
// 策略本身作为数据源
var generator = new JsonReportGenerator();

using var stream = File.Create("report.json");
await generator.Generate(strategy, stream, CancellationToken.None);
```

对于一个单独的数据源：

```csharp
var source = new ReportSource();
source.Name = strategy.Name;
source.PnL = strategy.PnL;
source.TotalWorkingTime = strategy.TotalWorkingTime;

// 从跟踪器添加持仓
source.AddPositions(tracker.History);

var generator = new CsvReportGenerator();
using var stream = File.Create("report.csv");
await generator.Generate(source, stream, CancellationToken.None);
```

## 示例：在停止时生成报告的策略

```csharp
public class ReportingStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public ReportingStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // 交易逻辑...
    }

    protected override void OnStopped()
    {
        // 策略停止时生成报表
        var generator = new CsvReportGenerator();

        using var stream = File.Create($"report_{Name}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        generator.Generate(this, stream, CancellationToken.None).AsTask().Wait();

        base.OnStopped();
    }
}
```

在这个例子中，该策略在停止时会自动创建一个 CSV 报告。报告包含策略参数、统计数据、订单、交易和持仓来回交易信息。
