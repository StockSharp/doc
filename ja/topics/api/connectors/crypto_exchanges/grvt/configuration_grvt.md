# GRVT コネクタ設定

サービスから発行された接続パラメーターをコネクター設定に指定します。

## 接続パラメーター

- `Key` - 認証に使用する API キー。
- `Secret` - 認証に使用する API シークレット。
- `SubAccountId` - 取引サブアカウントの識別子。
- `Environment` - サービス環境（本番ネットワークまたはテストネットワーク）。
- `EdgeEndpoint` - 認証 API のエンドポイント。
- `MarketDataEndpoint` - 市場データ API のエンドポイント。
- `TradingEndpoint` - 取引 API のエンドポイント。
- `MarketWebSocketEndpoint` - 市場データ用 WebSocket エンドポイント。
- `TradingWebSocketEndpoint` - 取引用 WebSocket エンドポイント。
- `SnapshotInterval` - 市場データのスナップショット間隔（ミリ秒）。
- `MarketDepth` - 取得する板情報のレベル数。

## 公式 API ドキュメント

- [公式 API ドキュメント 1](https://api-docs.grvt.io/)
- [公式 API ドキュメント 2](https://api-docs.grvt.io/auth/)
- [公式 API ドキュメント 3](https://api-docs.grvt.io/trading_api/)
