# 价差套利策略

## 概览

`MqSpreadStrategy`是一种策略，通过同时下达买卖报价在市场上创建价差。它使用两个报价处理器来管理市场两边的订单。

## 主要组件

```cs
public class MqSpreadStrategy : Strategy
{
	private readonly StrategyParam<MarketPriceTypes> _priceType;
	private readonly StrategyParam<Unit> _priceOffset;
	private readonly StrategyParam<Unit> _bestPriceOffset;

	private QuotingProcessor _buyProcessor;
	private QuotingProcessor _sellProcessor;
}
```

## 策略参数

该策略允许自定义以下参数：

- **PriceType** - 报价的市场价格类型（默认 跟随）
- **价格偏移** - 与市场价格的偏移
- **BestPriceOffset** - 报价更新的最小偏差（默认 0.1%）

## 策略初始化

在 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，该策略订阅市场时间的变化：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Subscribe to market time changes for quote updates
	Connector.CurrentTimeChanged += Connector_CurrentTimeChanged;
	Connector_CurrentTimeChanged(new TimeSpan());
}
```

## 管理报价处理器

当市场时间变化时，将调用 `Connector_CurrentTimeChanged` 方法，并管理报价处理器的创建和更新：

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
	// Create new processors only with zero position and if current ones are stopped
	if (Position != 0)
		return;

	if (_buyProcessor != null && _buyProcessor.LeftVolume > 0)
		return;

	if (_sellProcessor != null && _sellProcessor.LeftVolume > 0)
		return;

	// Release resources of existing processors
	_buyProcessor?.Dispose();
	_buyProcessor = null;

	_sellProcessor?.Dispose();
	_sellProcessor = null;

	// Create behaviors for market quoting
	var buyBehavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	var sellBehavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	// Create processor for buying
	_buyProcessor = new QuotingProcessor(
		buyBehavior,
		Security,
		Portfolio,
		Sides.Buy,
		Volume,
		Volume, // Maximum order volume
		TimeSpan.Zero, // No timeout
		this, // Strategy implements ISubscriptionProvider
		this, // Strategy implements IMarketRuleContainer
		this, // Strategy implements ITransactionProvider
		this, // Strategy implements ITimeProvider
		this, // Strategy implements IMarketDataProvider
		IsFormedAndOnlineAndAllowTrading, // Check trading permission
		true, // Use order book prices
		true  // Use last trade price if the order book is empty
	)
	{
		Parent = this
	};

	// Create processor for selling
	_sellProcessor = new QuotingProcessor(
		sellBehavior,
		Security,
		Portfolio,
		Sides.Sell,
		Volume,
		Volume, // Maximum order volume
		TimeSpan.Zero, // No timeout
		this, // Strategy implements ISubscriptionProvider
		this, // Strategy implements IMarketRuleContainer
		this, // Strategy implements ITransactionProvider
		this, // Strategy implements ITimeProvider
		this, // Strategy implements IMarketDataProvider
		IsFormedAndOnlineAndAllowTrading, // Check trading permission
		true, // Use order book prices
		true  // Use last trade price if the order book is empty
	)
	{
		Parent = this
	};

	// Log creation of new quoting processors
	this.AddInfoLog($"Created buy/sell spread at {CurrentTime}");

	// Subscribe to buy processor events for logging
	_buyProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Buy order {order.TransactionId} registered at price {order.Price}");

	_buyProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Buy order failed: {fail.Error.Message}");

	_buyProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Buy trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

	_buyProcessor.Finished += isOk => {
		this.AddInfoLog($"Buy quoting finished with success: {isOk}");
		_buyProcessor?.Dispose();
		_buyProcessor = null;
	};

	// Subscribe to sell processor events for logging
	_sellProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Sell order {order.TransactionId} registered at price {order.Price}");

	_sellProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Sell order failed: {fail.Error.Message}");

	_sellProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Sell trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

	_sellProcessor.Finished += isOk => {
		this.AddInfoLog($"Sell quoting finished with success: {isOk}");
		_sellProcessor?.Dispose();
		_sellProcessor = null;
	};

	// Start both processors
	_buyProcessor.Start();
	_sellProcessor.Start();
}
```

## 资源释放

在 [OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped) 方法中，该策略会释放资源：

```cs
protected override void OnStopped()
{
	// Unsubscribe to prevent memory leaks
	Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

	// Release processor resources
	_buyProcessor?.Dispose();
	_buyProcessor = null;

	_sellProcessor?.Dispose();
	_sellProcessor = null;

	base.OnStopped();
}
```

## 交易逻辑

- 该策略应对市场时间变化
- 在零位置和停止的处理器下，创建了两个新的处理器：
  - 购买处理器（购买）
  - 出售处理器（出售）
- 两个处理器都配置了相同的音量并使用相同的报价设置
- 处理器通过同时下买单和卖单在市场上制造价差

## 特征

- 使用现代报价处理器 [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) 和 [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)
- 通过同时下买单和卖单在市场中制造价差
- 仅适用于零持仓，防止积累不必要的风险
- 支持配置各种报价参数（价格类型、偏移量、最小偏差）
- 包括报价处理器事件的详细日志记录
- 在停止策略和创建新处理器时，正确管理资源
- 支持处理不同类型的市场价格（跟随价、最佳价、相反价等）