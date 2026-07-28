# コネクタ設定: Coincall

Coincall に接続する前に、次のアダプタープロパティを設定します。この一覧は [CoincallMessageAdapter](xref:StockSharp.Coincall.CoincallMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ProductType` (`CoincallProductTypes`)
- `RestEndpoint` (`string`)
- `OptionsWebSocketEndpoint` (`string`)
- `FuturesWebSocketEndpoint` (`string`)
- `RequestValidityWindow` (`TimeSpan`)
- `PrivatePollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_coincall.md)

[アダプターの初期化](adapter_initialization_coincall.md)
