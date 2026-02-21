# Compression of Tick Data and Spreads into Candles

## Introduction

The API provides powerful tools for compressing tick data and spreads (best bid/ask prices) into candles. This functionality is especially useful for analyzing historical data or building custom indicators.

The main extension methods for data compression are located in the `CandleHelper` class. The full source code for this class is [available on GitHub](https://github.com/StockSharp/StockSharp/blob/master/Algo/Candles/CandleHelper.cs).

It is recommended to review this file for a complete understanding of all available methods and their parameters.

## Compression Methods

### Tick Data Compression into Candles

```cs
// Example usage of ToCandles for ticks
var tickStorage = storageRegistry.GetTickMessageStorage(securityId, Drive, StorageFormat);
var trades = tickStorage.LoadAsync(from, to);
var candles = trades.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

// This code loads tick data from storage and converts it into candles.
// mdMsg - the message with parameters of the created candles (type, time frame, etc.).
// candleBuilderProvider - the provider that supplies a specific candle builder implementation.
```

### Spread Data Compression into Candles

```cs
// Example usage of ToCandles for spread data
var depthStorage = storageRegistry.GetQuoteMessageStorage(securityId, Drive, StorageFormat);
var depths = depthStorage.LoadAsync(from, to);
var candles = depths.ToCandles(mdMsg, Level1Fields.SpreadMiddle, candleBuilderProvider: candleBuilderProvider);

// Here we load spread data and convert it into candles.
// Level1Fields.SpreadMiddle indicates using the spread middle price for building candles.
// You can also use Level1Fields.BestBid or Level1Fields.BestAsk for the best bid or ask prices, respectively.
```

## Compression Parameters

When compressing data, the following parameters can be specified:

- `series`: The candle series defining the type and parameters of the created candles.
- `type`: The type of data for forming candles (e.g., best bid, best ask, or spread middle).
- `candleBuilderProvider`: The provider for the candle builder (optional parameter).

## Usage Example

```cs
private IEnumerable<CandleMessage> InternalGetCandles(SecurityId securityId, DateTime? from, DateTime? to)
{
	// ... (initialization code)

	switch (type)
	{
		case BuildTypes.Ticks:
			return StorageRegistry
					.GetTickMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

		case BuildTypes.OrderLog:
			// ... (code for building candles from order log)

		case BuildTypes.Depths:
			return StorageRegistry
					.GetQuoteMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, Convert(extraType), candleBuilderProvider: candleBuilderProvider);

		// ... (other cases)
	}
}

// This method demonstrates various ways to build candles depending on the type of source data.
// It supports building from ticks, order log, spreads, and other sources.
```

## Additional Features

### Building Candles from Various Sources

The API allows building candles not only from ticks and spreads but also from other data sources:

```cs
// Example of building candles from various sources
switch (type)
{
	case BuildTypes.Ticks:
		// ... (code for ticks)

	case BuildTypes.OrderLog:
		// ... (code for order log)

	case BuildTypes.Depths:
		// ... (code for spreads)

	case BuildTypes.Level1:
		// ... (code for Level1)

	case BuildTypes.SmallerTimeFrame:
		return candleBuilderProvider
				.GetCandleMessageBuildableStorage(StorageRegistry, securityId, mdMsg.GetTimeFrame(), Drive, StorageFormat)
				.LoadAsync(from, to);

	// ... (other cases)
}

// This code shows how to build candles from different data sources: ticks, order log, spreads, Level1 data, and even from smaller time frame candles.
```

## Conclusion

The data compression methods in the API provide flexible tools for working with market data. They allow for the efficient conversion of tick data and spread data into candles of various types and time intervals, which is particularly useful for market analysis and developing trading strategies.