# コネクタ設定: Birdeye

Birdeye に接続する前に、次のアダプタープロパティを設定します。この一覧は [BirdeyeMessageAdapter](xref:StockSharp.Birdeye.BirdeyeMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `WebSocketOrigin` (`string`)
- `Chain` (`string`)
- `TokenAddress` (`string`)
- `StreamingEnabled` (`bool`)
- `PriceInUsd` (`bool`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `MinimumLiquidity` (`decimal`)
- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_birdeye.md)

[アダプターの初期化](adapter_initialization_birdeye.md)
