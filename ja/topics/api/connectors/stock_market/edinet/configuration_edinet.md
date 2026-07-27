# コネクタ設定: EDINET

EDINET に接続する前に、次のアダプタープロパティを設定します。この一覧は [EdinetMessageAdapter](xref:StockSharp.Edinet.EdinetMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Address` (`Uri`)
- `CodeListAddress` (`Uri`)
- `ViewerAddress` (`Uri`)
- `DisclosureType` (`EdinetDisclosureTypes`)
- `ListedOnly` (`bool`)
- `IncludeWithdrawn` (`bool`)
- `IncludeUnavailable` (`bool`)
- `DefaultLookupDays` (`int`)
- `MaxDays` (`int`)
- `RequestInterval` (`TimeSpan`)
- `MaxDocumentSizeMb` (`int`)
- `CodeListCacheTimeout` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_edinet.md)

[アダプターの初期化](adapter_initialization_edinet.md)
