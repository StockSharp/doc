# コネクタ設定: KRX Open API

KRX Open API に接続する前に、次のアダプタープロパティを設定します。この一覧は [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`KrxDataSets`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `MaxRequests` (`int`)
- `Address` (`Uri`)
- `SampleAddress` (`Uri`)

## 関連項目

[グラフィカル設定](graphical_configuration_krx_open_api.md)

[アダプターの初期化](adapter_initialization_krx_open_api.md)
