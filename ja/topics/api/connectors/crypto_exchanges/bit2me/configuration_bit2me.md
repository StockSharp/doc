# Bit2Me コネクターの設定

公開市場データは認証情報なしで利用できます。口座および取引操作を使用する場合は、API キーとシークレットを指定します。

## 接続パラメーター

- `Key` — Bit2Me API キー。
- `Secret` — Bit2Me API シークレット。
- `RestEndpoint` — REST API アドレス。本番の既定値は `https://gateway.bit2me.com` です。
- `WebSocketEndpoint` — WebSocket アドレス。本番の既定値は `wss://ws.bit2me.com/v1/trading` です。

成行、指値、ストップリミット注文をサポートします。公開 WebSocket は約定と完全な Level 2 オーダーブック更新を配信し、ローソク足は REST から取得します。

## 公式 API ドキュメント

- [Bit2Me API](https://api.bit2me.com/)
- [Bit2Me 取引サンプル](https://github.com/bit2me-devs/trading-spot-samples)
