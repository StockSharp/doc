# Quoting Algorithm

## Overview

A quoting algorithm is a mechanism that allows automatically placing and updating orders in the market with the aim of achieving the best execution price. Instead of placing aggressive market orders, quoting uses limit orders, which helps minimize slippage and reduce trading costs.

## Main Components

StockSharp provides two key components for quoting:

1. **[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor)** — the main processor that analyzes market data and order status to recommend quoting actions.

2. **[IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior)** — an interface that defines quoting behavior, including calculating the optimal price and determining the need to update orders.

## Examples of Strategies with Quoting

The documentation presents examples of strategies that demonstrate the use of the quoting mechanism:

- **MqStrategy** — an example of a strategy that uses the quoting mechanism to manage a position in the market.
- **MqSpreadStrategy** — an example of a strategy that creates a spread in the market by simultaneously placing buy and sell quotes.
- **StairsCountertrendStrategy** — an example of a countertrend strategy using quoting for more precise market entry.

These examples help to understand how to integrate the quoting mechanism into your own trading algorithms.

## Quoting Behaviors

StockSharp supports various quoting behaviors:

- **[MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)** — quoting based on market price with customizable offset and type.
- **[BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior)** — quoting based on the best price with customizable offset.
- **[LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior)** — quoting at a fixed price.
- **[BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior)** — quoting based on the best price by volume.
- **[LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior)** — quoting based on a specified level in the order book.
- **[LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior)** — quoting based on the price of the last trade.
- **[TheorPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior)** — quoting options based on theoretical price.
- **[VolatilityQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior)** — quoting options based on volatility.

## Use in Your Own Strategy

### Step 1: Create a Quoting Behavior

```csharp
// Create a behavior for market quoting
var behavior = new MarketQuotingBehavior(
    new Unit(0.01m), // Price offset from the best quote
    new Unit(0.1m, UnitTypes.Percent), // Minimum deviation for quote update
    MarketPriceTypes.Following // Market price type for quoting
);
```

### Step 2: Create and Initialize the Processor

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

### Step 3: Subscribe to Processor Events

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

### Step 4: Start the Processor

```csharp
// Start the processor
_quotingProcessor.Start();
```

### Step 5: Release Resources

Don't forget to clean up processor resources when stopping the strategy:

```csharp
protected override void OnStopped()
{
    // Release resources of the current processor
    _quotingProcessor?.Dispose();
    _quotingProcessor = null;
    
    base.OnStopped();
}
```

## Advantages of Usage

1. **Reduced Slippage** — quoting helps to get a better execution price compared to market orders.

2. **Configuration Flexibility** — various quoting behaviors for different market situations.

3. **Automatic Updates** — the processor automatically tracks market changes and updates orders when necessary.

4. **Full Control** — ability to configure quoting parameters, including price offset, minimum deviation, and market price type.

5. **Risk Management** — ability to set a timeout to limit execution time.

## Applied Use Cases

- **Market Making** — creating liquidity in the market by placing two-sided quotes.
- **Algorithmic Trading** — improving execution price in automated strategies.
- **Countertrend Trading** — more precise market entry against the trend.
- **Arbitrage** — simultaneous quoting on multiple markets to take advantage of price discrepancies.

Implementing the quoting mechanism in your strategy can significantly improve order execution and increase its efficiency.