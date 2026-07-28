# コネクタ設定: DEX Screener

DEX Screener に接続する前に、次のアダプタープロパティを設定します。この一覧は [DexScreenerMessageAdapter](xref:StockSharp.DexScreener.DexScreenerMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `RestEndpoint` (`string`)
- `ChainId` (`string`)
- `TokenAddress` (`string`)
- `SearchQuery` (`string`)
- `PriceInUsd` (`bool`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_dex_screener.md)

[アダプターの初期化](adapter_initialization_dex_screener.md)
