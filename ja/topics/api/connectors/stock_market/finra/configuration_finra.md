# コネクタ設定: FINRA

FINRA に接続する前に、次のアダプタープロパティを設定します。この一覧は [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`FinraDataSets`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Token` (`SecureString`)
- `WeeklyTierIdentifier` (`string`)
- `WeeklySummaryTypeCode` (`string`)
- `PageSize` (`int`)
- `MaxRecords` (`int`)
- `DataVersion` (`int`)
- `Address` (`Uri`)
- `AuthAddress` (`Uri`)

## 関連項目

[グラフィカル設定](graphical_configuration_finra.md)

[アダプターの初期化](adapter_initialization_finra.md)
