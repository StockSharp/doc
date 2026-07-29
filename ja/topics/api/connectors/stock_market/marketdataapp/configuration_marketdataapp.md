# コネクタ設定: MarketData.app

MarketData.app に接続する前に、次のアダプタープロパティを設定します。この一覧は [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)
- `RestEndpoint` (`Uri`)

## 詳細設定

これらのプロパティは、接続先、要求間隔、フィルター、データオプション、結果上限を制御します。

- `ExtendedHours` (`bool`)
- `AdjustSplits` (`bool`)
- `MaximumOptionContracts` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_marketdataapp.md)

[アダプターの初期化](adapter_initialization_marketdataapp.md)
