# コネクタ設定: SEC API

SEC API に接続する前に、次のアダプタープロパティを設定します。この一覧は [SecApiMessageAdapter](xref:StockSharp.SecApi.SecApiMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)
- `Address` (`Uri`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `ActiveOnly` (`bool`)
- `DefaultExchange` (`string`)
- `FormTypes` (`string`)
- `ResultLimit` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_sec_api.md)

[アダプターの初期化](adapter_initialization_sec_api.md)
