# コネクタ設定: DeepBook

DeepBook に接続する前に、次のアダプタープロパティを設定します。この一覧は [DeepBookMessageAdapter](xref:StockSharp.DeepBook.DeepBookMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `IndexerEndpoint` (`string`)
- `GrpcEndpoint` (`string`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `PackageId` (`string`)
- `ClockObjectId` (`string`)
- `Pools` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLimit` (`int`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_deepbook.md)

[アダプターの初期化](adapter_initialization_deepbook.md)
