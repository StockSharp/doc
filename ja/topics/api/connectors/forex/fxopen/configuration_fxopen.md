# コネクタ設定: FXOpen TickTrader

FXOpen Web API トークンを作成し、接続パラメーターを指定します。

- `WebApiId` - Web API トークン ID。
- `Key` - Web API キー。
- `Secret` - Web API シークレット。
- `OneTimePassword` - 2 要素認証が必要な場合の任意のワンタイムパスワード。
- `IsDemo` - デモ環境を選択します。既定値は `false` です。
- `Address` - REST エンドポイント。ライブ既定値は `https://ttlivewebapi.fxopen.net`。
- `FeedAddress` - Feed WebSocket。ライブ既定値は `wss://marginalttlivewebapi.fxopen.net/feed`。
- `TradeAddress` - Trade WebSocket。ライブ既定値は `wss://marginalttlivewebapi.fxopen.net/trade`。

`IsDemo` を有効にすると、アドレスを手動変更していない限り公式 TickTrader デモエンドポイントが選択されます。WebSocket 購読と非公開操作には ID、キー、シークレットが必要です。

## 関連項目

[FXOpen 公式 API ドキュメント](https://ticktrader.fxopen.com/api)

[TickTrader Web REST API](https://ttlivewebapi.fxopen.net/api/doc/index?apiaddress=ttlivewebapi.fxopen.net&apiport=443)
