# コネクタ設定: ESMA FIRDS

ESMA FIRDS に接続する前に、次のアダプタープロパティを設定します。この一覧は [EsmaFirdsMessageAdapter](xref:StockSharp.EsmaFirds.EsmaFirdsMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Address` (`Uri`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `CfiCategories` (`string`)
- `ActiveOnly` (`bool`)
- `MaxResults` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_esma_firds.md)

[アダプターの初期化](adapter_initialization_esma_firds.md)
