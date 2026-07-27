# コネクタ設定: TPEx

TPEx に接続する前に、次のアダプタープロパティを設定します。この一覧は [TpexMessageAdapter](xref:StockSharp.Tpex.TpexMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Address` (`Uri`)
- `Market` (`TpexMarkets`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `IncludeListedDerivatives` (`bool`)
- `IncludeValuations` (`bool`)
- `CacheTimeout` (`TimeSpan`)
- `MaxHistoryMonths` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_tpex.md)

[アダプターの初期化](adapter_initialization_tpex.md)
