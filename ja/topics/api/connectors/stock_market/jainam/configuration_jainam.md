# コネクタ設定: Jainam

Jainam に接続する前に、次のアダプタープロパティを設定します。この一覧は [JainamMessageAdapter](xref:StockSharp.Jainam.JainamMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `UserId` (`string`)
- `AppCode` (`string`)
- `ApiSecret` (`SecureString`)
- `AuthCode` (`SecureString`)
- `Token` (`SecureString`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `PortfolioName` (`string`)
- `DefaultProduct` (`JainamProducts`)
- `ReconnectAttempts` (`int`)
- `PollingInterval` (`TimeSpan`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`string`)
- `WebSocketAddress` (`string`)

## 関連項目

[グラフィカル設定](graphical_configuration_jainam.md)

[アダプターの初期化](adapter_initialization_jainam.md)
