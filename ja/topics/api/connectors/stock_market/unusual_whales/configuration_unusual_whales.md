# コネクタ設定: Unusual Whales

Unusual Whales に接続する前に、次のアダプタープロパティを設定します。この一覧は [UnusualWhalesMessageAdapter](xref:StockSharp.UnusualWhales.UnusualWhalesMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)
- `Address` (`Uri`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `CandleLimit` (`int`)
- `NewsLimit` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `UnusualFlowOnly` (`bool`)
- `OtmMarketTide` (`bool`)
- `FiveMinuteMarketTide` (`bool`)

## 関連項目

[グラフィカル設定](graphical_configuration_unusual_whales.md)

[アダプターの初期化](adapter_initialization_unusual_whales.md)
