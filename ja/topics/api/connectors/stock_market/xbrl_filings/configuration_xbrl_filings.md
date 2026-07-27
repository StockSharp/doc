# コネクタ設定: XBRL Filings

XBRL Filings に接続する前に、次のアダプタープロパティを設定します。この一覧は [XbrlFilingsMessageAdapter](xref:StockSharp.XbrlFilings.XbrlFilingsMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Address` (`Uri`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `PublicAddress` (`Uri`)
- `Country` (`string`)
- `PageSize` (`int`)
- `MaxPages` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_xbrl_filings.md)

[アダプターの初期化](adapter_initialization_xbrl_filings.md)
