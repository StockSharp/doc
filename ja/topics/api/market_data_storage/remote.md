# リモートストレージの操作

## はじめに

ローカルストレージに加えて、API はリモート市場データストレージを扱う機能を提供します。これは、[サーバーモード](../../hydra/server_mode/settings.md) の Hydra を使用する場合や、[Hydra サーバー](../../hydra_server.md) に接続する場合に特に便利です。

## リモートストレージへの接続

リモートストレージを扱うには、[RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive) クラスを使用します。

```cs
// RemoteMarketDataDrive を作成します
var remoteDrive = new RemoteMarketDataDrive(RemoteMarketDataDrive.DefaultAddress, new FixMessageAdapter(new IncrementalIdGenerator()))
{
	Credentials = { Email = "hydra_user", Password = "hydra_user".To<SecureString>() }
};

// このコードは、リモートストレージに接続するための RemoteMarketDataDrive のインスタンスを作成します。
// 通信には既定のアドレスと FixMessageAdapter を使用します。
// 認証のために資格情報を設定します。
```

## 銘柄情報の読み込み

市場データを読み込む前に、利用可能な銘柄に関する情報を取得する必要があります。

```cs
// 銘柄情報を読み込みます
var exchangeInfoProvider = new InMemoryExchangeInfoProvider();
remoteDrive.LookupSecurities(Extensions.LookupAllCriteriaMessage, registry.Securities,
	s => securityStorage.Save(s.ToSecurity(exchangeInfoProvider), false), () => false,
	(c, t) => Console.WriteLine($"Downloaded [{c}]/[{t}]"));

var securities = securityStorage.LookupAll();

// このコードは、リモートストレージから利用可能なすべての銘柄に関する情報を読み込みます。
// 読み込まれた銘柄はローカルストレージに保存され、コンソールに出力されます。
```

## 市場データの読み込み

銘柄情報を取得した後、市場データの読み込みに進むことができます。

```cs
// 市場データを読み込みます
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	var localStorage = storageRegistry.GetStorage(secId, dataType.MessageType, dataType.Arg, localDrive, format);
	var remoteStorage = remoteDrive.GetStorageDrive(secId, dataType, format);

	// ...（データ読み込みコード）
}

// このループは、指定した銘柄で利用可能なすべてのデータ型を反復処理します。
// 各データ型について、ローカルストレージを作成し、リモートストレージにアクセスします。
```

## ローカルへのデータ保存

読み込まれたデータは、後で使用するためにローカルに保存できます。

```cs
// データをローカルに保存します
foreach (var dateTime in dates)
{
	using (var stream = remoteStorage.LoadStream(dateTime))
	{
		if (stream == Stream.Null)
			continue;

		localStorage.Drive.SaveStream(dateTime, stream);
	}

	// ...（データ出力コード）
}

// このコードは、各日付のデータをリモートストレージから読み込み、ローカルストレージに保存します。
```

## テストでのデータ使用

読み込んでローカルに保存したデータは、[HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) を使用した取引ストラテジーのテストに使用できます。

```cs
// HistoryEmulationConnector を使用します
var connector = new HistoryEmulationConnector(secProvider, new[] { pf }, new StorageRegistry { DefaultDrive = remoteDrive });
```

## 利用可能な日付範囲の取得

```cs
// さまざまなデータ型を扱います
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	// ...（データ処理コード）

	// エラー処理とログ記録
	Console.WriteLine($"Remote {dataType}: {remoteStorage.Dates.FirstOrDefault()}-{remoteStorage.Dates.LastOrDefault()}");
	Console.WriteLine($"{dataType}={dateTime}");
}
```

## まとめ

API のリモート市場データストレージ機能は、ヒストリカルデータを取得して使用するための柔軟な機能を提供します。これにより、[Hydra サーバー](../../hydra_server.md) を通じて利用可能な大規模データセットを使用して、取引ストラテジーの効率的なテストと市場分析を行えます。

