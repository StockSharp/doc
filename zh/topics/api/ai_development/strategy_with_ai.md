# 使用 AI 编写策略

本章节分步介绍如何借助 AI 工具创建 StockSharp 交易策略。

## 准备工作

### 1. 安装 AI 工具

选择一种可用工具：

- **Claude Code** — `npm install -g @anthropic-ai/claude-code`（需要 Node.js）
- **Cursor** — 从 [cursor.com](https://cursor.com) 下载
- **GitHub Copilot** — 为所用 IDE 安装插件

### 2. 创建项目

```bash
dotnet new console -n MyStrategy --framework net10.0
cd MyStrategy
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies
dotnet add package StockSharp.Algo.Indicators
dotnet add package StockSharp.Algo.Testing
dotnet add package StockSharp.Binance
```

### 3. 准备上下文

在项目根目录创建 `CLAUDE.md`（或 `.cursorrules`）文件：

```markdown
# 项目规则

- Framework: StockSharp 5.x, .NET 10
- Strategies inherit from Strategy class
- Subscribe to candles via Connector.Subscribe(subscription)
- Register orders via RegisterOrder(order)
- Logging: this.AddInfoLog(), this.AddWarningLog(), this.AddErrorLog()
- Indicators: create via new and call indicator.Process(candle)
- Always handle connector.Error and strategy errors
```

## 分步示例：SMA 策略

### 第 1 步：向 AI 描述任务

提示词示例：

```
Create a trading strategy using StockSharp that:
- Inherits from Strategy
- Uses two simple moving averages (SMA): fast (period 10) and slow (period 30)
- When the fast SMA crosses above the slow SMA — buy
- When the fast SMA crosses below the slow SMA — sell
- Position size: 1 lot
- Uses 5-minute candles
- Subscribes to candles in OnStarted()
- Processes candles via subscription rules
```

### 第 2 步：检查生成的代码

AI 会生成类似以下内容的代码：

```csharp
using StockSharp.Algo;
using StockSharp.Algo.Indicators;
using StockSharp.Algo.Strategies;
using StockSharp.BusinessEntities;
using StockSharp.Messages;

public class SmaCrossStrategy : Strategy
{
    private readonly StrategyParam<int> _fastPeriod;
    private readonly StrategyParam<int> _slowPeriod;
    private readonly StrategyParam<DataType> _candleType;

    private SimpleMovingAverage _fastSma;
    private SimpleMovingAverage _slowSma;

    private bool _prevFastAbove;
    private bool _isFirstCandle = true;

    public SmaCrossStrategy()
    {
        _fastPeriod = Param(nameof(FastPeriod), 10);
        _slowPeriod = Param(nameof(SlowPeriod), 30);
        _candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(5)));
    }

    public int FastPeriod
    {
        get => _fastPeriod.Value;
        set => _fastPeriod.Value = value;
    }

    public int SlowPeriod
    {
        get => _slowPeriod.Value;
        set => _slowPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    protected override void OnStarted(DateTimeOffset time)
    {
        base.OnStarted(time);

        _fastSma = new SimpleMovingAverage { Length = FastPeriod };
        _slowSma = new SimpleMovingAverage { Length = SlowPeriod };
        _isFirstCandle = true;

        var subscription = new Subscription(CandleType, Security);

        this
            .WhenCandlesFinished(subscription)
            .Do(ProcessCandle)
            .Apply(this);

        Subscribe(subscription);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        var fastValue = _fastSma.Process(candle);
        var slowValue = _slowSma.Process(candle);

        if (!_fastSma.IsFormed || !_slowSma.IsFormed)
            return;

        var fastAbove = fastValue.GetValue<decimal>() > slowValue.GetValue<decimal>();

        if (!_isFirstCandle)
        {
            if (fastAbove && !_prevFastAbove)
            {
                // 快速 SMA 上穿慢速 SMA — 买入
                if (Position <= 0)
                    RegisterOrder(this.BuyAtMarket(Volume));
            }
            else if (!fastAbove && _prevFastAbove)
            {
                // 快速 SMA 下穿慢速 SMA — 卖出
                if (Position >= 0)
                    RegisterOrder(this.SellAtMarket(Volume));
            }
        }

        _prevFastAbove = fastAbove;
        _isFirstCandle = false;
    }
}
```

### 第 3 步：检查要点

按以下清单检查代码：

- **继承**：类继承自 `Strategy` ✓
- **参数**：使用 `StrategyParam<T>`，以便进行优化 ✓
- **K线订阅**：通过 `Subscribe(new Subscription(...))` 完成 ✓
- **K线处理**：通过 `WhenCandlesFinished` 规则完成 ✓
- **IsFormed 检查**：检查指标是否已经形成 ✓
- **订单**：通过 `RegisterOrder()` 和 `BuyAtMarket` / `SellAtMarket` 提交 ✓
- **持仓**：提交订单前检查 `Position` ✓

### 第 4 步：要求 AI 添加回测

```
Add backtesting code for this strategy using historical data.
Use HistoryEmulationConnector, load data from local storage,
and output summary statistics (PnL, trade count, max drawdown).
```

## 提示词示例

### 布林带策略

```
Create a StockSharp strategy that trades using Bollinger Bands:
- Buy when price touches the lower band
- Sell when price touches the upper band
- Period 20, multiplier 2.0
- Stop-loss: 1% from entry price
- Take-profit: 2% from entry price
- Use StrategyParam for all parameters
```

### 套利策略

```
Create a pairs arbitrage strategy on StockSharp:
- Two instruments (specified via parameters)
- Calculate the spread between prices
- Enter when spread deviates by 2 standard deviations
- Exit when spread returns to the mean
- Volume neutralization (equal positions in monetary terms)
```

### 市场深度短线策略

```
Create a scalping strategy on StockSharp:
- Subscribe to order book (MarketDepth) via Subscribe
- Analyze bid/ask imbalance
- Enter on strong imbalance (> 3:1)
- Quick exit on take-profit (5 ticks)
- Stop-loss: 3 ticks
- Maximum 1 position at a time
```

## AI 常见错误

### 1. 使用过时的事件

**错误**（旧版 API）：

```csharp
connector.NewSecurities += securities => { ... };
connector.CandleSeriesProcessing += (series, candle) => { ... };
```

**正确**（当前 API）：

```csharp
// 使用订阅
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security);
connector.Subscribe(subscription);
```

### 2. 不使用辅助方法创建订单

**错误**：

```csharp
var order = new Order
{
    Security = Security,
    Portfolio = Portfolio,
    Side = Sides.Buy,
    Type = OrderTypes.Market,
    Volume = 1,
};
```

**正确**（使用策略辅助方法）：

```csharp
RegisterOrder(this.BuyAtMarket(Volume));
// or
RegisterOrder(this.SellAtLimit(price, Volume));
```

### 3. 缺少 IsFormed 检查

**错误**：

```csharp
var value = _sma.Process(candle);
// Using value immediately — may not be ready
```

**正确**：

```csharp
var value = _sma.Process(candle);
if (!_sma.IsFormed)
    return;
```

## 建议

1. **向 AI 提供文档** — 引导其查看 [doc.stocksharp.com](https://doc.stocksharp.com)，或复制 `Samples/` 中的代码示例
2. **使用 CLAUDE.md** — 项目规则文件可以显著减少错误
3. **从简单策略开始** — 先创建基础策略，再添加筛选条件和风险管理
4. **使用历史数据测试** — 实盘交易前务必先运行回测
5. **克隆仓库** — 如果 AI 能够访问 StockSharp 源代码，就能更准确地使用 API
