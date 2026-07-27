# コネクタ設定: MasterLink

MasterLink に接続する前に、次のアダプタープロパティを設定します。この一覧は [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Login` (`string`)
- `Password` (`SecureString`)
- `CertificatePath` (`string`)
- `CertificatePassword` (`SecureString`)
- `NodePath` (`string`)
- `GatewayDirectory` (`string`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Account` (`string`)
- `RegisterApiAuth` (`bool`)
- `MarketDataMode` (`MasterLinkMarketDataModes`)
- `AdjustedCandles` (`bool`)
- `AccountPollingInterval` (`TimeSpan`)
- `MaxLookupResults` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_masterlink.md)

[アダプターの初期化](adapter_initialization_masterlink.md)
