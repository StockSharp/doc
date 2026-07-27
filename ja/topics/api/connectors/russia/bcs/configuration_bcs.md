# コネクタ設定: BCS

BCS に接続する前に、次のアダプタープロパティを設定します。この一覧は [BcsMessageAdapter](xref:StockSharp.Bcs.BcsMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)
- `IsReadOnly` (`bool`)
- `PortfolioName` (`string`)
- `PollingInterval` (`TimeSpan`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## 関連項目

[グラフィカル設定](graphical_configuration_bcs.md)

[アダプターの初期化](adapter_initialization_bcs.md)
