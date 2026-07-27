# コネクタ設定: Korean FSC

Korean FSC に接続する前に、次のアダプタープロパティを設定します。この一覧は [KoreanFscMessageAdapter](xref:StockSharp.KoreanFsc.KoreanFscMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)
- `DataSet` (`KoreanFscDataSets`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Address` (`Uri`)
- `Market` (`KoreanFscMarkets`)
- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `PageSize` (`int`)
- `MaxPages` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_korean_fsc.md)

[アダプターの初期化](adapter_initialization_korean_fsc.md)
