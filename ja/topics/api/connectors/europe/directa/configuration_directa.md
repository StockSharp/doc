# コネクタ設定: Directa

Directa に接続する前に、次のアダプタープロパティを設定します。この一覧は [DirectaMessageAdapter](xref:StockSharp.Directa.DirectaMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Address` (`EndPoint`)
- `DataAddress` (`EndPoint`)
- `HistoryAddress` (`EndPoint`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `RequestTimeout` (`TimeSpan`)
- `AutoConfirmOrders` (`bool`)
- `MaxMarketDepth` (`int`)
- `TimeZoneId` (`string`)

## 関連項目

[グラフィカル設定](graphical_configuration_directa.md)

[アダプターの初期化](adapter_initialization_directa.md)
