# Quoting Strategy

## Overview

`MqStrategy` is a strategy that uses a quoting mechanism to manage market positions. It creates a quoting processor based on the current position, allowing it to adaptively respond to market condition changes.

## Main Components

```cs
public class MqStrategy : Strategy
{
    private readonly StrategyParam<MarketPriceTypes> _priceType;
    private readonly StrategyParam<Unit> _priceOffset;
    private readonly StrategyParam<Unit> _bestPriceOffset;

    private QuotingProcessor _quotingProcessor;
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
    Connector_CurrentTimeChanged(default);
}
```

## Managing the Quoting Processor

The `Connector_CurrentTimeChanged` method is called when the market time changes and manages the creation and update of the quoting processor:

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
    // Create a new processor only if the current one is stopped or doesn't exist
    if (_quotingProcessor != null && _quotingProcessor.LeftVolume > 0)
        return;

    // Release resources of the old processor if it exists
    _quotingProcessor?.Dispose();
    _quotingProcessor = null;

    // Determine quoting side based on current position
    var side = Position <= 0 ? Sides.Buy : Sides.Sell;

    // Create new quoting behavior
    var behavior = new MarketQuotingBehavior(
        PriceOffset,
        BestPriceOffset,
        PriceType
    );

    // Calculate quoting volume
    var quotingVolume = Volume + Math.Abs(Position);

    // Create and initialize the processor
    _quotingProcessor = new QuotingProcessor(
        behavior,
        Security,
        Portfolio,
        side,
        quotingVolume,
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

    // Subscribe to processor events for logging
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

    // Start the processor
    _quotingProcessor.Start();
}
```

## Resource Release

In the [OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped) method, the strategy releases resources:

```cs
protected override void OnStopped()
{
    // Unsubscribe to prevent memory leaks
    Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

    // Release resources of the current processor if it exists
    _quotingProcessor?.Dispose();
    _quotingProcessor = null;

    base.OnStopped();
}
```

## Trading Logic

- The strategy responds to market time changes
- Quoting direction is determined based on the current position:
  - If position <= 0, a Buy quote is created
  - If position > 0, a Sell quote is created
- Quoting volume is calculated as the base volume plus the absolute value of the current position
- [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) with [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) is used for quoting

## Features

- Uses modern quoting processor instead of legacy quoting strategies
- Adaptively responds to position changes by changing quoting direction
- Supports configuration of various quoting parameters (price type, offset, minimum deviation)
- Includes detailed logging of quoting processor events
- Properly manages resources when stopping the strategy and creating new processors
- Supports working with different types of market prices (Following, Best, Opposite, etc.)