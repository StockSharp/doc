# コネクタ設定: Open DART

Open DART に接続する前に、次のアダプタープロパティを設定します。この一覧は [OpenDartMessageAdapter](xref:StockSharp.OpenDart.OpenDartMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Address` (`Uri`)
- `DisclosureAddress` (`Uri`)
- `DisclosureType` (`OpenDartDisclosureTypes`)
- `CorporationClass` (`OpenDartCorporationClasses`)
- `FinalReportsOnly` (`bool`)
- `BusinessYear` (`int?`)
- `ReportType` (`OpenDartReportTypes`)
- `FinancialSearchYears` (`int`)
- `MaxPages` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_open_dart.md)

[アダプターの初期化](adapter_initialization_open_dart.md)
