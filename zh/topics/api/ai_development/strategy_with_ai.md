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

- 框架：StockSharp 5.x、.NET 10
- 策略继承 Strategy 类
- 通过 Connector.Subscribe(subscription) 订阅 K线
- 通过 RegisterOrder(order) 注册订单
- 日志记录：this.AddInfoLog()、this.AddWarningLog()、this.AddErrorLog()
- 指标：使用 new 创建，并调用 indicator.Process(candle)
- 始终处理 connector.Error 和策略错误
```

## 分步示例：SMA 策略

### 第 1 步：向 AI 描述任务

提示词示例：

```
使用 StockSharp 创建满足以下条件的交易策略：
- 继承 Strategy
- 使用两条简单移动平均线（SMA）：fast（周期 10）和 slow（周期 30）
- 当快速 SMA 上穿慢速 SMA 时买入
- 当快速 SMA 下穿慢速 SMA 时卖出
- 持仓规模：1 手
- 使用 5 分钟 K线
- 在 OnStarted() 中订阅 K线
- 通过订阅规则处理 K线
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
创建一个使用布林带交易的 StockSharp 策略：
- 当价格触及下轨时买入
- 当价格触及上轨时卖出
- 周期 20，乘数 2.0
- 止损：距入场价 1%
- 止盈：距入场价 2%
- 所有参数都使用 StrategyParam
```

### 套利策略

```
在 StockSharp 上创建配对套利策略：
- 两个交易品种（通过参数指定）
- 计算价格之间的价差
- 当价差偏离 2 个标准差时入场
- 当价差回归均值时退出
- 数量中性化（按金额保持等额持仓）
```

### 市场深度短线策略

```
创建一个 StockSharp 剥头皮策略：
- 通过 Subscribe 订阅订单簿（MarketDepth）
- 分析买卖盘不平衡
- 在强不平衡（> 3:1）时入场
- 快速止盈退出（5 个 tick）
- 止损：3 个 tick
- 同一时间最多 1 个持仓
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
