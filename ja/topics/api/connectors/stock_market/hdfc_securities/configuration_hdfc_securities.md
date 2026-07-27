# コネクタ設定: HDFC Securities

HDFC Securities に接続する前に、次のアダプタープロパティを設定します。この一覧は [HdfcMessageAdapter](xref:StockSharp.HdfcSecurities.HdfcMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RequestToken` (`SecureString`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Token` (`SecureString`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`HdfcProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)

## 関連項目

[グラフィカル設定](graphical_configuration_hdfc_securities.md)

[アダプターの初期化](adapter_initialization_hdfc_securities.md)
