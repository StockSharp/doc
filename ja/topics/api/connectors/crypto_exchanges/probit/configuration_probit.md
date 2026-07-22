# ProBit Global コネクター設定

コネクター設定で ProBit Global の接続パラメーターを指定します。

## 接続パラメーター

- `Key` - ProBit API 認証情報で発行された OAuth クライアント ID。
- `Secret` - OAuth クライアントシークレット。
- `RestEndpoint` - REST API のアドレス。
- `AuthEndpoint` - OAuth トークンエンドポイントのアドレス。
- `WebSocketEndpoint` - WebSocket サーバーのアドレス。

公開市場データには認証情報は不要です。取引、残高、注文履歴、プライベート WebSocket チャネルには `Key` と `Secret` が必要です。

成行買いでは、決済通貨の金額を `ProBitOrderCondition.QuoteAmount` に設定します。

## 公式 API ドキュメント

- [ProBit Global API ドキュメント](https://docs-en.probit.com/)
- [ProBit Global API 認証情報](https://www.probit.com/en-us/my-page/api-management/api-credential)
