# コネクタ設定: Deriv

Deriv トークンを作成し、接続パラメーターを指定します。

- `Token` - パーソナルアクセストークンまたは OAuth トークン。非公開操作に必要です。
- `AppId` - 認証付き REST リクエストとともに送信されるアプリケーション識別子。
- `AccountId` - オプション口座の識別子。選択したモードに一致する有効な口座がちょうど 1 つの場合は任意です。
- `IsDemo` - デモ口座を選択します。既定値は `true` です。
- `RestAddress` - REST エンドポイント。既定値は `https://api.derivws.com`。
- `PublicWebSocketAddress` - 公開オプション WebSocket エンドポイント。既定値は `wss://api.derivws.com/trading/v1/options/ws/public`。

公開市場データのセッションはトークンなしで動作しますが、契約、残高、トランザクションにはトークンとアプリケーション ID が必要です。サブスクリプションは、新しいワンタイム WebSocket アドレスを通じて自動的に復元されます。

契約パラメーターは [DerivOrderCondition](xref:StockSharp.Deriv.DerivOrderCondition) を通じて渡されます。契約タイプ、金額が賭け金か払戻金か、契約通貨、期間、バリア、保護用のストップロス価格とテイクプロフィット価格です。

## 関連項目

[Deriv 公式 API ドキュメント](https://developers.deriv.com/docs/)
