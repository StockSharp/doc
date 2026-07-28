# コネクタ設定: Pendle

Pendle に接続する前に、次のアダプタープロパティを設定します。この一覧は [PendleMessageAdapter](xref:StockSharp.Pendle.PendleMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Chain` (`PendleChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## 詳細設定

これらのプロパティは、市場の選択、制限、定期照会、トランザクション動作を制御します。

- `MarketAddresses` (`string`)
- `MaxMarkets` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `HistoryLimit` (`int`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## 関連項目

[グラフィカル設定](graphical_configuration_pendle.md)

[アダプターの初期化](adapter_initialization_pendle.md)
