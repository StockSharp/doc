# 使用 API

## 准备

在示例中处理历史数据时，使用了包含历史数据样本的 NuGet 包。它可以从 [NuGet 画廊](https://www.nuget.org/packages/StockSharp.Samples.HistoryData) 安装。此包提供了一组可用于演示存储操作的数据。

所有代码都可以在 [StockSharp 仓库](https://github.com/StockSharp/StockSharp/tree/master/Samples/03_Storage) 中获取。

## 创建存储注册表

在 StockSharp 中处理市场数据存储时，使用 [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) 类。创建此类对象时，可以通过 [StorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.StorageRegistry.DefaultDrive) 属性设置默认存储的路径，或使用 [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) 指定用于处理历史数据的特定文件夹。

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

## 正在检索数据

通过 [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry)，您可以访问所需时间范围内的各种类型的市场数据。用于此的方法有：

- [StorageRegistry.GetTimeFrameCandleMessageStorage](xref:StockSharp.Algo.Storages.StorageHelper.GetTimeFrameCandleMessageStorage(StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Messages.SecurityId,System.TimeSpan,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) 用于蜡烛
- [StorageRegistry.GetTickMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetTickMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) 用于蜱虫
- [StorageRegistry.GetQuoteMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetQuoteMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,System.Boolean)) 用于订单簿

每种方法都会返回相应的存储，可以使用 `LoadAsync` 方法从中加载数据，指定开始和结束日期。

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

## 保存数据

要将新数据保存到现有存储中，请使用相应存储的 `SaveAsync` 方法。这使您可以用新值补充历史数据。

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

## 删除数据

要删除特定期间的数据，请使用相应存储的 `DeleteAsync` 方法。在删除样本包中的数据时要小心。

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

这些操作允许您有效管理历史数据，无论是通过 [Hydra](../../hydra.md) 加载、在 NuGet 软件包中提供，还是在您的应用程序运行过程中创建的。