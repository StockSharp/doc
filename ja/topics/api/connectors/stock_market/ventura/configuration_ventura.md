# コネクタ設定: Ventura

Ventura に接続する前に、次のアダプタープロパティを設定します。この一覧は [VenturaMessageAdapter](xref:StockSharp.Ventura.VenturaMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `ClientId` (`string`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `RequestToken` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `Pin` (`SecureString`)
- `TotpSecret` (`SecureString`)
- `MacAddress` (`string`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`VenturaProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `MarketDataAddress` (`Uri`)
- `OrderStatusAddress` (`Uri`)

## 関連項目

[グラフィカル設定](graphical_configuration_ventura.md)

[アダプターの初期化](adapter_initialization_ventura.md)
