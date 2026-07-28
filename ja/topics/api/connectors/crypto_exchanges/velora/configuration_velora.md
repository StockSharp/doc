# コネクタ設定: Velora

Velora に接続する前に、次のアダプタープロパティを設定します。この一覧は [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Partner` (`string`)
- `Chain` (`VeloraChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Markets` (`string`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## 関連項目

[グラフィカル設定](graphical_configuration_velora.md)

[アダプターの初期化](adapter_initialization_velora.md)
