# コネクタ設定: CoinSwitch PRO

CoinSwitch PRO に接続する前に、次のアダプタープロパティを設定します。この一覧は [CoinSwitchMessageAdapter](xref:StockSharp.CoinSwitch.CoinSwitchMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ProductType` (`CoinSwitchProductTypes`)
- `SpotExchange` (`string`)
- `RestEndpoint` (`string`)
- `HftEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_coinswitch.md)

[アダプターの初期化](adapter_initialization_coinswitch.md)
