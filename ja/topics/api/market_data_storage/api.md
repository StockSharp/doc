# API の操作

## 準備

例でヒストリカルデータを扱うために、ヒストリカルデータサンプルを含む NuGet パッケージを使用します。これは [NuGet Gallery](https://www.nuget.org/packages/StockSharp.Samples.HistoryData) からインストールできます。このパッケージは、ストレージ操作のデモに使用できるデータセットを提供します。

すべてのコードは [StockSharp リポジトリ](https://github.com/StockSharp/StockSharp/tree/master/Samples/03_Storage) で入手できます。

## ストレージレジストリの作成

StockSharp で市場データストレージを扱うには、[StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) クラスを使用します。このクラスのオブジェクトを作成するときは、[StorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.StorageRegistry.DefaultDrive) プロパティを介して既定ストレージへのパスを設定するか、[LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) を使用してヒストリカルデータを扱う特定のフォルダーを指定できます。

```cs
// 既定パスで StorageRegistry を作成します
var storageRegistry = new StorageRegistry();
```

```cs
// NuGet パッケージのデータへのパスを指定して StorageRegistry を作成します
var pathHistory = Paths.HistoryDataPath; // NuGet パッケージのデータへのパス
var localDrive = new LocalMarketDataDrive(pathHistory);
var storageRegistry = new StorageRegistry()
{
	DefaultDrive = localDrive,
};
```

## データの取得

[StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) を通じて、目的の時間範囲に対するさまざまな種類の市場データにアクセスできます。このために使用されるメソッドは次のとおりです。

- [StorageRegistry.GetTimeFrameCandleMessageStorage](xref:StockSharp.Algo.Storages.StorageHelper.GetTimeFrameCandleMessageStorage(StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Messages.SecurityId,System.TimeSpan,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) はキャンドル用です
- [StorageRegistry.GetTickMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetTickMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) はティック用です
- [StorageRegistry.GetQuoteMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetQuoteMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,System.Boolean)) は板情報用です

これらの各メソッドは対応するストレージを返し、開始日と終了日を指定して `LoadAsync` メソッドを使用することで、そこからデータを読み込めます。

```cs
// キャンドルを取得します
var securityId = "AAPL@NASDAQ".ToSecurityId();
var candleStorage = storageRegistry.GetTimeFrameCandleMessageStorage(securityId, TimeSpan.FromMinutes(1), StorageFormats.Binary);
var candles = candleStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var candle in candles)
{
	Console.WriteLine(candle);
}
```

```cs
// ティックを取得します
var tradeStorage = storageRegistry.GetTickMessageStorage(securityId, StorageFormats.Binary);
var trades = tradeStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var trade in trades)
{
	Console.WriteLine(trade);
}
```

```cs
// 板情報を取得します
var marketDepthStorage = storageRegistry.GetQuoteMessageStorage(securityId, StorageFormats.Binary);
var marketDepths = marketDepthStorage.LoadAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

await foreach (var marketDepth in marketDepths)
{
	Console.WriteLine(marketDepth);
}
```

## データの保存

既存のストレージに新しいデータを保存するには、対応するストレージの `SaveAsync` メソッドを使用します。これにより、ヒストリカルデータに新しい値を追加できます。

```cs
// 新しいキャンドルを保存します
var newCandles = new List<CandleMessage>
{
	// ここで新しい CandleMessage オブジェクトを作成します
};
await candleStorage.SaveAsync(newCandles);
```

```cs
// 新しいティックを保存します
var newTrades = new List<ExecutionMessage>
{
	// ここでティック用の新しい ExecutionMessage オブジェクトを作成します
};
await tradeStorage.SaveAsync(newTrades);
```

```cs
// 新しい板情報を保存します
var newMarketDepths = new List<QuoteChangeMessage>
{
	// ここで板情報用の新しい QuoteChangeMessage オブジェクトを作成します
};
await marketDepthStorage.SaveAsync(newMarketDepths);
```

## データの削除

特定期間のデータを削除するには、対応するストレージの `DeleteAsync` メソッドを使用します。サンプルパッケージからデータを削除するときは注意してください。

```cs
// 指定した期間のキャンドルを削除します
await candleStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// 指定した期間のティックを削除します
await tradeStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// 指定した期間の板情報を削除します
await marketDepthStorage.DeleteAsync(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

これらの操作により、[Hydra](../../hydra.md) を通じて読み込まれたもの、NuGet パッケージで提供されたもの、またはアプリケーションの動作中に作成されたもののいずれであっても、ヒストリカルデータを効率的に管理できます。

