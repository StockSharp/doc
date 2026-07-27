# コネクタ設定: Rupeezy

Rupeezy に接続する前に、次のアダプタープロパティを設定します。この一覧は [RupeezyMessageAdapter](xref:StockSharp.Rupeezy.RupeezyMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `ApplicationId` (`string`)
- `ApiKey` (`SecureString`)
- `AuthCode` (`SecureString`)
- `Token` (`SecureString`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `PortfolioName` (`string`)
- `DefaultProduct` (`RupeezyProducts`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_rupeezy.md)

[アダプターの初期化](adapter_initialization_rupeezy.md)
