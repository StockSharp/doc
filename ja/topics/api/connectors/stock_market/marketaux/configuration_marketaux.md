# コネクタ設定: Marketaux

Marketaux に接続する前に、次のアダプタープロパティを設定します。この一覧は [MarketauxMessageAdapter](xref:StockSharp.Marketaux.MarketauxMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)
- `Address` (`Uri`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Languages` (`string`)
- `EntityTypes` (`string`)
- `Countries` (`string`)
- `MustHaveEntities` (`bool`)
- `GroupSimilar` (`bool`)
- `NewsPageSize` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `SentimentInterval` (`MarketauxIntervals`)

## 関連項目

[グラフィカル設定](graphical_configuration_marketaux.md)

[アダプターの初期化](adapter_initialization_marketaux.md)
