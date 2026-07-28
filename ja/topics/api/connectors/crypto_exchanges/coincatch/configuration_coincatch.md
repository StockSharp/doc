# コネクタ設定: CoinCatch

CoinCatch に接続する前に、次のアダプタープロパティを設定します。この一覧は [CoinCatchMessageAdapter](xref:StockSharp.CoinCatch.CoinCatchMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Passphrase` (`SecureString`)
- `ProductType` (`CoinCatchProductTypes`)
- `RestEndpoint` (`string`)
- `PublicWebSocketEndpoint` (`string`)
- `PrivateWebSocketEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_coincatch.md)

[アダプターの初期化](adapter_initialization_coincatch.md)
