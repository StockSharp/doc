# コネクタ設定: GLEIF

GLEIF に接続する前に、次のアダプタープロパティを設定します。この一覧は [GleifMessageAdapter](xref:StockSharp.Gleif.GleifMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Address` (`Uri`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `ActiveOnly` (`bool`)
- `ExpandIsins` (`bool`)
- `PageSize` (`int`)
- `MaxPages` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_gleif.md)

[アダプターの初期化](adapter_initialization_gleif.md)
