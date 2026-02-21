# Working with the API

## Preparation

For working with historical data in the examples, a NuGet package with historical data samples is used. It can be installed from the [NuGet Gallery](https://www.nuget.org/packages/StockSharp.Samples.HistoryData). This package provides a set of data that can be used to demonstrate working with the storage.

All codes are available in the [StockSharp repository](https://github.com/StockSharp/StockSharp/tree/master/Samples/03_Storage).

## Creating a Storage Registry

To work with market data storage in StockSharp, the [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) class is used. When creating an object of this class, you can set the path to the default storage via the [StorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.StorageRegistry.DefaultDrive) property or specify a specific folder for working with historical data using the [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive).

```cs
// Creating StorageRegistry with default path
var storageRegistry = new StorageRegistry();
```

```cs
// Creating StorageRegistry with the path to data from the NuGet package
var pathHistory = Paths.HistoryDataPath; // path to data from the NuGet package
var localDrive = new LocalMarketDataDrive(pathHistory);
var storageRegistry = new StorageRegistry()
{
	DefaultDrive = localDrive,
};
```

## Retrieving Data

Through the [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry), you can access various types of market data for the desired time range. The methods used for this are:

- [StorageRegistry.GetTimeFrameCandleMessageStorage](xref:StockSharp.Algo.Storages.StorageHelper.GetTimeFrameCandleMessageStorage(StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Messages.SecurityId,System.TimeSpan,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) for candles
- [StorageRegistry.GetTickMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetTickMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) for ticks
- [StorageRegistry.GetQuoteMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetQuoteMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,System.Boolean)) for order books

Each of these methods returns the corresponding storage, from which data can be loaded using the `LoadAsync` method, specifying the start and end dates.

```cs
// Retrieving candles
var securityId = "SBER@TQBR".ToSecurityId();
var candleStorage = storageRegistry.GetTimeFrameCandleMessageStorage(securityId, TimeSpan.FromMinutes(1), StorageFormats.Binary);
var candles = candleStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var candle in candles)
{
	Console.WriteLine(candle);
}
```

```cs
// Retrieving ticks
var tradeStorage = storageRegistry.GetTickMessageStorage(securityId, StorageFormats.Binary);
var trades = tradeStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var trade in trades)
{
	Console.WriteLine(trade);
}
```

```cs
// Retrieving order books
var marketDepthStorage = storageRegistry.GetQuoteMessageStorage(securityId, StorageFormats.Binary);
var marketDepths = marketDepthStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var marketDepth in marketDepths)
{
	Console.WriteLine(marketDepth);
}
```

## Saving Data

To save new data to the existing storage, use the `SaveAsync` method of the corresponding storage. This allows you to supplement historical data with new values.

```cs
// Saving new candles
var newCandles = new List<CandleMessage>
{
	// New CandleMessage objects are created here
};
await candleStorage.SaveAsync(newCandles);
```

```cs
// Saving new ticks
var newTrades = new List<ExecutionMessage>
{
	// New ExecutionMessage objects for ticks are created here
};
await tradeStorage.SaveAsync(newTrades);
```

```cs
// Saving new order books
var newMarketDepths = new List<QuoteChangeMessage>
{
	// New QuoteChangeMessage objects for order books are created here
};
await marketDepthStorage.SaveAsync(newMarketDepths);
```

## Deleting Data

To delete data for a specific period, use the `DeleteAsync` method of the corresponding storage. Be careful when deleting data from the sample package.

```cs
// Deleting candles for the specified period
await candleStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// Deleting ticks for the specified period
await tradeStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// Deleting order books for the specified period
await marketDepthStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

These operations allow you to effectively manage historical data, whether loaded through [Hydra](../../hydra.md), provided in the NuGet package, or created during the operation of your application.