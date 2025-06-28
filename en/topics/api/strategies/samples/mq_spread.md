# Spread Quoting Strategy

## Overview

`MqSpreadStrategy` is a strategy that creates a spread in the market by simultaneously placing quotes for buying and selling. It uses two quoting processors to manage orders on both sides of the market.

## Main Components

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

## Strategy Parameters

The strategy allows customizing the following parameters:

- **PriceType** - market price type for quoting (default Following)
- **PriceOffset** - price offset from the market price
- **BestPriceOffset** - minimum deviation for quote update (default 0.1%)

## Strategy Initialization

In the [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) method, the strategy subscribes to market time changes:

```cs
protected override void OnStarted(DateTimeOffset time)
{
	base.OnStarted(time);

	// Subscribe to market time changes for quote updates
	Connector.CurrentTimeChanged += Connector_CurrentTimeChanged;
	Connector_CurrentTimeChanged(new TimeSpan());
}
```

## Managing Quoting Processors

The `Connector_CurrentTimeChanged` method is called when the market time changes and manages the creation and update of quoting processors:

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

## Resource Release

In the [OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped) method, the strategy releases resources:

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

## Trading Logic

- The strategy responds to market time changes
- With zero position and stopped processors, two new processors are created:
  - Buy processor (Buy)
  - Sell processor (Sell)
- Both processors are configured with the same volume and use the same quoting settings
- Processors create a spread in the market by simultaneously placing buy and sell orders

## Features

- Uses modern quoting processor [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) with [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)
- Creates a spread in the market by simultaneously placing buy and sell orders
- Works only with zero position, preventing accumulation of unwanted risk
- Supports configuration of various quoting parameters (price type, offset, minimum deviation)
- Includes detailed logging of quoting processor events
- Properly manages resources when stopping the strategy and creating new processors
- Supports working with different types of market prices (Following, Best, Opposite, etc.)