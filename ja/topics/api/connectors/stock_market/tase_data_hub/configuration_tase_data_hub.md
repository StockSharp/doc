# コネクタ設定: TASE Data Hub

TASE Data Hub に接続する前に、次のアダプタープロパティを設定します。この一覧は [TaseDataHubMessageAdapter](xref:StockSharp.TaseDataHub.TaseDataHubMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Address` (`Uri`)
- `Scope` (`string`)
- `SecurityLookupDays` (`int`)
- `ReferenceCacheTimeout` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_tase_data_hub.md)

[アダプターの初期化](adapter_initialization_tase_data_hub.md)
