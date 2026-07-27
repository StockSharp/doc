# コネクタ設定: Tradejini

Tradejini に接続する前に、次のアダプタープロパティを設定します。この一覧は [TradejiniMessageAdapter](xref:StockSharp.Tradejini.TradejiniMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `ApiKey` (`SecureString`)
- `Password` (`SecureString`)
- `TwoFactorCode` (`SecureString`)
- `TwoFactorType` (`TradejiniTwoFactorTypes`)
- `Token` (`SecureString`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `PortfolioName` (`string`)
- `DefaultProduct` (`TradejiniProducts`)
- `Address` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_tradejini.md)

[アダプターの初期化](adapter_initialization_tradejini.md)
