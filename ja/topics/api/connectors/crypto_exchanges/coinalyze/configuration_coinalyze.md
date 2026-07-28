# コネクタ設定: Coinalyze

Coinalyze に接続する前に、次のアダプタープロパティを設定します。この一覧は [CoinalyzeMessageAdapter](xref:StockSharp.Coinalyze.CoinalyzeMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `MarketType` (`CoinalyzeMarketTypes`)
- `CandleMetric` (`CoinalyzeCandleMetrics`)
- `Exchange` (`string`)
- `ConvertToUsd` (`bool`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `RequestInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_coinalyze.md)

[アダプターの初期化](adapter_initialization_coinalyze.md)
