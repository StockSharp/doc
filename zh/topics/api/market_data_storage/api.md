# 使用 API

## 准备

在示例中处理历史数据时，使用了包含历史数据样本的 NuGet 包。它可以从 [NuGet 画廊](https://www.nuget.org/packages/StockSharp.Samples.HistoryData) 安装。此包提供了一组可用于演示存储操作的数据。

所有代码都可以在 [StockSharp 仓库](https://github.com/StockSharp/StockSharp/tree/master/Samples/03_Storage) 中获取。

## 创建存储注册表

在 StockSharp 中处理市场数据存储时，使用 [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) 类。创建此类对象时，可以通过 [StorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.StorageRegistry.DefaultDrive) 属性设置默认存储的路径，或使用 [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) 指定用于处理历史数据的特定文件夹。

```cs
// 使用默认路径创建 StorageRegistry
var storageRegistry = new StorageRegistry();
```

```cs
// 使用 NuGet 包数据路径创建 StorageRegistry
var pathHistory = Paths.HistoryDataPath; // NuGet 包中数据的路径
var localDrive = new LocalMarketDataDrive(pathHistory);
var storageRegistry = new StorageRegistry()
{
	DefaultDrive = localDrive,
};
```

## 正在检索数据

通过 [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry)，您可以访问所需时间范围内的各种类型的市场数据。用于此的方法有：

- [StorageRegistry.GetTimeFrameCandleMessageStorage](xref:StockSharp.Algo.Storages.StorageHelper.GetTimeFrameCandleMessageStorage(StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Messages.SecurityId,System.TimeSpan,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) 用于K线
- [StorageRegistry.GetTickMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetTickMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) 用于蜱虫
- [StorageRegistry.GetQuoteMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetQuoteMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,System.Boolean)) 用于订单簿

每种方法都会返回相应的存储，可以使用 `LoadAsync` 方法从中加载数据，指定开始和结束日期。

```cs
// 获取 K线
var securityId = "AAPL@NASDAQ".ToSecurityId();
var candleStorage = storageRegistry.GetTimeFrameCandleMessageStorage(securityId, TimeSpan.FromMinutes(1), StorageFormats.Binary);
var candles = candleStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var candle in candles)
{
	Console.WriteLine(candle);
}
```

```cs
// 获取 tick
var tradeStorage = storageRegistry.GetTickMessageStorage(securityId, StorageFormats.Binary);
var trades = tradeStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var trade in trades)
{
	Console.WriteLine(trade);
}
```

```cs
// 获取订单簿
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
// 保存新 K线
var newCandles = new List<CandleMessage>
{
	// 在这里创建新的 CandleMessage 对象
};
await candleStorage.SaveAsync(newCandles);
```

```cs
// 保存新 tick
var newTrades = new List<ExecutionMessage>
{
	// 在这里创建用于 tick 的新 ExecutionMessage 对象
};
await tradeStorage.SaveAsync(newTrades);
```

```cs
// 保存新订单簿
var newMarketDepths = new List<QuoteChangeMessage>
{
	// 在这里创建用于订单簿的新 QuoteChangeMessage 对象
};
await marketDepthStorage.SaveAsync(newMarketDepths);
```

## 删除数据

要删除特定期间的数据，请使用相应存储的 `DeleteAsync` 方法。在删除样本包中的数据时要小心。

```cs
// 删除指定期间的 K线
await candleStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// 删除指定期间的 tick
await tradeStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// 删除指定期间的订单簿
await marketDepthStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

这些操作允许您有效管理历史数据，无论是通过 [Hydra](../../hydra.md) 加载、在 NuGet 软件包中提供，还是在您的应用程序运行过程中创建的。
