# コネクタ設定: Wisdom Capital

Wisdom Capital に接続する前に、次のアダプタープロパティを設定します。この一覧は [WisdomCapitalMessageAdapter](xref:StockSharp.WisdomCapital.WisdomCapitalMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `MarketDataKey` (`SecureString`)
- `MarketDataSecret` (`SecureString`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Token` (`SecureString`)
- `UserId` (`string`)
- `MarketDataToken` (`SecureString`)
- `MarketDataUserId` (`string`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`WisdomCapitalProducts`)
- `Source` (`string`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `EngineIoVersion` (`int`)
- `RestAddress` (`Uri`)

## 関連項目

[グラフィカル設定](graphical_configuration_wisdom_capital.md)

[アダプターの初期化](adapter_initialization_wisdom_capital.md)
