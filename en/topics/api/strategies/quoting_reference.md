# Quoting Reference

## Overview

This section contains a complete reference of the quoting system architecture and components in StockSharp. All available quoting behaviors, action types, and interfaces are described. For a basic introduction, see [Quoting Algorithm](quoting.md).

## Architecture

### QuotingStrategy (Deprecated)

The [QuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.QuotingStrategy) class is deprecated. It is recommended to use [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) instead.

Main parameters of the deprecated class:

- `QuotingSide` -- quoting direction (Buy/Sell)
- `QuotingVolume` -- quoting volume
- `TimeOut` -- execution timeout
- `UseBidAsk` -- use order book prices
- `UseLastTradePrice` -- use last trade price

### QuotingEngine

[QuotingEngine](xref:StockSharp.Algo.Strategies.Quoting.QuotingEngine) -- the functional core of the quoting system. It calculates remaining volume and timeouts, returning action recommendations without side effects.

### QuotingBehaviorAlgo

[QuotingBehaviorAlgo](xref:StockSharp.Algo.Strategies.Quoting.QuotingBehaviorAlgo) -- implementation of the `IPositionModifyAlgo` interface for algorithmic position management.

Main methods:

| Method | Description |
|--------|-------------|
| `UpdateMarketData(time, price, volume)` | Update market data |
| `UpdateOrderBook(depth)` | Update order book |
| `GetNextAction()` | Get the next quoting action |

Supports VWAP and TWAP modes.

### QuotingProcessor

[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) -- the main processor for executing quoting within a strategy. Manages the order lifecycle: placement, modification, cancellation.

## QuotingAction Types

The processor returns actions of type [QuotingAction](xref:StockSharp.Algo.Strategies.Quoting.QuotingAction):

| Type | Description |
|------|-------------|
| `None(reason)` | No action required |
| `PlaceOrder(price, volume)` | Place a new order |
| `ModifyOrder(price, volume)` | Modify an existing order |
| `CancelOrder()` | Cancel the order |
| `Finish(success, reason)` | Finish quoting |

## Quoting Behaviors

StockSharp provides 10 types of quoting behaviors, each implementing the [IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior) interface:

| # | Class | Description | Key Parameters |
|---|-------|-------------|----------------|
| 1 | [BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior) | Best price from order book | BestPriceOffset |
| 2 | [LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior) | Last trade price | BestPriceOffset |
| 3 | [LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior) | Fixed limit price | LimitPrice |
| 4 | [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) | Market price with offset | PriceOffset, PriceType (Following/Opposite/Middle) |
| 5 | [VolatilityQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior) | Option volatility (Black-Scholes) | IVRange, Model |
| 6 | [TheorPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior) | Option theoretical price | TheorPriceOffset |
| 7 | [BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior) | Cumulative volume in order book | VolumeExchange |
| 8 | [LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior) | Order book depth level | Level (Range\<int\>), OwnLevel |
| 9 | [VWAPQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VWAPQuotingBehavior) | VWAP -- volume-weighted average | BestPriceOffset |
| 10 | [TWAPQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TWAPQuotingBehavior) | TWAP -- time-weighted average | TimeInterval, PriceBufferSize (default 10) |

## IQuotingBehavior Interface

The [IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior) interface defines two key methods:

### CalculateBestPrice

Calculates the best price for placing an order:

```cs
decimal? CalculateBestPrice(
	Security security,
	IMarketDataProvider provider,
	Sides quotingDirection,
	decimal? bestBid,
	decimal? bestAsk,
	decimal? lastTradePrice,
	decimal? lastTradeVolume,
	IEnumerable<QuoteChange> bids,
	IEnumerable<QuoteChange> asks);
```

### NeedQuoting

Determines whether the current order needs to be updated:

```cs
decimal? NeedQuoting(
	Security security,
	IMarketDataProvider provider,
	DateTimeOffset currentTime,
	decimal? currentPrice,
	decimal? currentVolume,
	decimal? newVolume,
	decimal? bestPrice);
```

Returns `null` if no update is needed, or the new price for placing the order.

## Usage Examples

### Market Quoting with Offset

```cs
var behavior = new MarketQuotingBehavior
{
	PriceOffset = new Unit(2, UnitTypes.Absolute),
	PriceType = MarketPriceTypes.Following,
	BestPriceOffset = new Unit(0.5m),
};
```

### VWAP Quoting

```cs
var behavior = new VWAPQuotingBehavior
{
	BestPriceOffset = new Unit(1),
};
```

### Option Volatility Quoting

```cs
var behavior = new VolatilityQuotingBehavior
{
	IVRange = new Range<decimal>(0.2m, 0.3m),
	Model = new BlackScholes(option, connector),
};
```

### Full Example with Processor

```cs
// Choose the quoting behavior
var behavior = new BestByPriceQuotingBehavior
{
	BestPriceOffset = new Unit(0.01m),
};

// Create the processor
var processor = new QuotingProcessor(
	behavior,
	Security,
	Portfolio,
	Sides.Buy,
	volume: 10,
	maxOrderVolume: 10,
	timeout: TimeSpan.FromMinutes(5),
	subscriptionProvider: this,
	ruleContainer: this,
	transactionProvider: this,
	timeProvider: this,
	marketDataProvider: this,
	isTradingAllowed: IsFormedAndOnlineAndAllowTrading,
	useBidAsk: true,
	useLastTradePrice: true)
{
	Parent = this,
};

processor.Finished += isOk =>
{
	this.AddInfoLog($"Quoting finished: {isOk}");
	processor?.Dispose();
};

processor.Start();
```

## See Also

- [Quoting Algorithm](quoting.md)
- [Quoting Strategy](samples/mq.md)
- [Volatility Quoting](../options/volatility_trading.md)
