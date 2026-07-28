# コネクタ設定: Settrade

Settrade に接続する前に、次のアダプタープロパティを設定します。この一覧は [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AppCode` (`string`)
- `BrokerId` (`string`)
- `Account` (`string`)
- `Pin` (`SecureString`)
- `AccountType` (`SettradeAccountTypes`)
- `IsDemo` (`bool`)

## 詳細設定

これらのプロパティは、ログインパラメーター、本番環境とテスト環境の接続先、非公開状態の定期照会を制御します。

- `LoginParameters` (`string`)
- `RestEndpoint` (`string`)
- `DemoRestEndpoint` (`string`)
- `MarketDataEndpoint` (`string`)
- `DemoMarketDataEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_settrade.md)

[アダプターの初期化](adapter_initialization_settrade.md)
