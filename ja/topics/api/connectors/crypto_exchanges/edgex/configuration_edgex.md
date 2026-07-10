# edgeX の設定

コネクターを使用するには、取引所アカウントで **APIキー** と **シークレット** を生成し、接続設定で指定します。

主な設定:

- **キー** と **シークレット**。
- **清算口座** と **パスフレーズ**。
- **セクション**: `Spot` または `Derivatives`。
- **スポットを有効化**: API サポートが利用可能な場合にスポットセクションを有効にします。
- **デモ** モード。
- **Spot REST / Derivatives REST** エンドポイント。
- **Spot WS / Derivatives public WS / Derivatives private WS** エンドポイント。

公式 API ドキュメント:

- [Authentication](https://edgex-1.gitbook.io/edgex-documentation/developer/api/authentication)
- [Order API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/order-api)
- [アカウント API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/account-api)
- [プライベート WebSocket ストリーム](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/private-websocket-stream)
- [Funding API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/funding-api)
- [メタデータ API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/meta-data-api)
- [Quote API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/quote-api)

> [!TIP]
> `Derivatives` は完全に実装されています。`Spot` は対象の API 環境がサポートしている場合にのみ有効にしてください。
