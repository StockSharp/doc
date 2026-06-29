# 报价算法

## 概览

报价算法是一种机制，它允许在市场中自动下单和更新订单，目的是获得最佳执行价格。报价使用限价单而不是积极的市价单，这有助于最小化滑点并降低交易成本。

## 主要组件

StockSharp 提供报价的两个关键组成部分：

1. **[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor)** —— 主要处理器，用于分析市场数据和订单状态，以推荐报价操作。

2. **[IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior)** — 一个定义报价行为的接口，包括计算最优价格和确定是否需要更新订单。

## 引用策略示例

文档展示了演示引用机制使用的策略示例：

- **MqStrategy** — 一个利用报价机制来管理市场头寸的策略示例。
- **MqSpreadStrategy** — 一个示例策略，通过同时下买入和卖出报价在市场上创建价差。
- **阶梯逆势策略** — 一个使用报价来实现更精确市场入场的逆势策略示例。

这些例子有助于理解如何将报价机制整合到你自己的交易算法中。

## 引用行为

StockSharp 支持各种引用行为：

- **[MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)** — 基于市场价格的报价，可自定义偏移量和类型。
- **[BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior)** — 基于最佳价格进行报价，并可自定义偏移量。
- **[LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior)** — 以固定价格报价。
- **[BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior)** — 根据成交量的最佳价格进行报价。
- **[LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior)** — 基于订单簿中指定级别的报价。
- **[LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior)** — 基于最后交易价格的报价。
- **[理论报价行为](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior)** — 基于理论价格的期权报价。
- **[波动率报价行为](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior)** — 基于波动率的期权报价。

## 在你自己的策略中使用

### 步骤 1：创建引用行为

```csharp
// Create a behavior for market quoting
var behavior = new MarketQuotingBehavior(
	new Unit(0.01m), // Price offset from the best quote
	new Unit(0.1m, UnitTypes.Percent), // Minimum deviation for quote update
	MarketPriceTypes.Following // Market price type for quoting
);
```

### 步骤 2：创建并初始化处理器

```csharp
// Create a quoting processor
_quotingProcessor = new QuotingProcessor(
	behavior,
	Security, // Instrument
	Portfolio, // Portfolio
	Sides.Buy, // Quoting direction
	Volume, // Quoting volume
	Volume, // Maximum order volume
	TimeSpan.Zero, // No timeout
	this, // Strategy implements ISubscriptionProvider
	this, // Strategy implements IMarketRuleContainer
	this, // Strategy implements ITransactionProvider
	this, // Strategy implements ITimeProvider
	this, // Strategy implements IMarketDataProvider
	IsFormedAndOnlineAndAllowTrading, // Check trading permission
	true, // Use order book prices
	true  // Use last trade price if order book is empty
)
{
	Parent = this
};
```

### 第3步：订阅处理器事件

```csharp
// Subscribe to processor events for logging and handling
_quotingProcessor.OrderRegistered += order =>
	this.AddInfoLog($"Order {order.TransactionId} registered at price {order.Price}");

_quotingProcessor.OrderFailed += fail =>
	this.AddInfoLog($"Order failed: {fail.Error.Message}");

_quotingProcessor.OwnTrade += trade =>
	this.AddInfoLog($"Trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

_quotingProcessor.Finished += isOk => {
	this.AddInfoLog($"Quoting finished with success: {isOk}");
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;
};
```

### 第4步：启动处理器

```csharp
// Start the processor
_quotingProcessor.Start();
```

### 步骤5：释放资源

在停止策略时，不要忘记清理处理器资源：

```csharp
protected override void OnStopped()
{
	// Release resources of the current processor
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;
	
	base.OnStopped();
}
```

## 使用的优点

1. **降低滑点** — 报价有助于相比市价单获得更好的执行价格。

2. **配置灵活性** — 针对不同市场情况的各种报价行为。

3. **自动更新** — 处理器会自动跟踪市场变化，并在必要时更新订单。

4. **完全控制** — 能够配置报价参数，包括价格偏移、最小偏差和市场价格类型。

5. **风险管理** — 设置超时以限制执行时间的能力。

## 应用案例

- **做市** — 通过提供双向报价来在市场中创造流动性。
- **算法交易** — 提高自动化策略中的执行价格。
- **逆势交易** — 更精确地在趋势相反时入市。
- **套利** — 在多个市场同时报价以利用价格差异。

在你的策略中实施报价机制可以显著改善订单执行并提高其效率。