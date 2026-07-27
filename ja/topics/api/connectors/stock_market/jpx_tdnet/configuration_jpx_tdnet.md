# コネクタ設定: JPX TDnet

JPX TDnet に接続する前に、次のアダプタープロパティを設定します。この一覧は [JpxTdnetMessageAdapter](xref:StockSharp.JpxTdnet.JpxTdnetMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Address` (`Uri`)
- `ViewerAddress` (`Uri`)
- `IndexMode` (`JpxTdnetIndexModes`)
- `DefaultLookupDays` (`int`)
- `MaxDays` (`int`)
- `SecurityLookupDays` (`int`)
- `RequestInterval` (`TimeSpan`)
- `MaxDocumentSizeMb` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_jpx_tdnet.md)

[アダプターの初期化](adapter_initialization_jpx_tdnet.md)
