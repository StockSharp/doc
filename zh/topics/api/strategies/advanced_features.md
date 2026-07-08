# 高级策略功能

## 概览

`Strategy` 类提供了许多额外的属性，用于微调行为：自动订单注释、交易计划、统计的无风险利率、指标的数据源以及历史时期管理。

## 评论模式 -- 评论排序

`CommentMode` 属性控制策略提交的所有订单的 `Order.Comment` 字段的自动填充。这有助于识别哪个策略创建了订单，这在同一账户上同时运行多个策略时尤其有用。

### StrategyCommentModes 枚举

| 值 | 描述 |
|-------|-------------|
| `Disabled` | 评论不会自动填充。默认值。 |
| `Id` | 评论已设置为 `Strategy.Id`（唯一 GUID 标识符）。 |
| `Name` | 评论已设置为 `Strategy.Name`（策略名称）。 |

### 例子

```csharp
public class CommentStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // 所有订单都会带有策略名称标签
        CommentMode = StrategyCommentModes.Name;

        // 或使用标识符进行精确绑定
        // CommentMode = StrategyCommentModes.Id;
    }
}
```

使用 `Name` 值和策略名称“SMA Crossover”，每个订单都会收到评论“SMA Crossover”，从而让您可以在交易日志中筛选该策略的订单。

## 工作时间 -- 工作安排

`WorkingTime` 属性设置策略处于活动状态的时间表。在指定时间间隔之外，策略可以自动限制其活动。

```csharp
public class ScheduledStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // 配置工作时间
        WorkingTime = new WorkingTime
        {
            Periods = new List<WorkingTimePeriod>
            {
                new WorkingTimePeriod
                {
                    Till = DateTime.MaxValue,
                    Times = new List<Range<TimeSpan>>
                    {
                        // 从 10:00 到 18:00 交易
                        new Range<TimeSpan>(
                            TimeSpan.FromHours(10),
                            TimeSpan.FromHours(18))
                    }
                }
            }
        };
    }
}
```

`TotalWorkingTime` 属性（只读）显示策略自启动以来的总工作时间。它会在策略停止和重启时自动计算。

## 无风险利率 -- 无风险利率

`RiskFreeRate` 属性设置在统计计算中使用的年度无风险利率——主要用于夏普比率和索提诺比率。

```csharp
var strategy = new MyStrategy();

// 年化 5% 的无风险利率
strategy.RiskFreeRate = 0.05m;
```

当策略的统计管理器初始化时，该值会自动传递给实现 `IRiskFreeRateStatisticParameter` 的所有统计参数。

## IndicatorSource -- 指标数据来源

`IndicatorSource` 属性为所有未明确指定来源的策略指标设置 `IIndicator.Source` 属性的默认值。它定义使用哪个 `Level1Fields` 字段作为指标输入数据。

```csharp
var strategy = new MyStrategy();

// 所有指标默认使用最新成交价
strategy.IndicatorSource = Level1Fields.LastTradePrice;

// 或平均价格
// strategy.IndicatorSource = Level1Fields.AveragePrice;
```

如果属性是 `null`（默认值），指示器使用它们自己的数据源。

## 历史计算 -- 计算的历史时期

虚拟属性 `HistoryCalculated` 允许策略以编程方式确定指标预热所需的历史数据周期。它返回 `TimeSpan?`——历史周期的持续时间，如果未指定周期，则返回 `null`。

```csharp
public class SmaCrossStrategy : Strategy
{
    private readonly StrategyParam<int> _longPeriod;

    public int LongPeriod
    {
        get => _longPeriod.Value;
        set => _longPeriod.Value = value;
    }

    public SmaCrossStrategy()
    {
        _longPeriod = Param(nameof(LongPeriod), 50);
    }

    // 自动计算所需历史期间
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(LongPeriod * 2);
}
```

`HistoryCalculated` 是 `HistorySize` 属性的代码计算版本。区别在于 `HistorySize` 由用户作为策略参数设置，而 `HistoryCalculated` 是根据策略参数（例如指标周期）通过程序计算得出的。

## 示例：包含所有高级设置的策略

```csharp
public class AdvancedStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
    private readonly StrategyParam<int> _smaPeriod;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public int SmaPeriod
    {
        get => _smaPeriod.Value;
        set => _smaPeriod.Value = value;
    }

    public AdvancedStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
        _smaPeriod = Param(nameof(SmaPeriod), 20);
    }

    // 自动计算历史期间
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(SmaPeriod * 2);

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // 订单注释 -- 策略名称
        CommentMode = StrategyCommentModes.Name;

        // 用于 Sharpe 计算的无风险利率
        RiskFreeRate = 0.05m;

        // 指标的数据源
        IndicatorSource = Level1Fields.LastTradePrice;

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
}
```

在这个例子中，该策略使用了所有描述的功能：它会自动对订单进行评论，为统计设置无风险利率，建立指标的数据来源，并计算所需的历史周期。
