# コネクタ設定: EXANTE

EXANTE に接続する前に、次のアダプタープロパティを設定します。この一覧は [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `SummaryCurrency` (`string`)
- `PollingInterval` (`TimeSpan`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `MaxMarketDepth` (`int`)
- `HistoryRequestSize` (`int`)
- `LiveAddress` (`Uri`)
- `DemoAddress` (`Uri`)

## 関連項目

[グラフィカル設定](graphical_configuration_exante.md)

[アダプターの初期化](adapter_initialization_exante.md)
