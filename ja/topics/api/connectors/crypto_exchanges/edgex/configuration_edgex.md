# edgeX の設定

コネクターを使用するには、取引所アカウントで **APIキー** と **シークレット** を生成し、接続設定で指定します。

主な設定:

- **キー** と **シークレット**。
- **清算口座** と **パスフレーズ**。
- **Section**: `Spot` または `Derivatives`。
- **Enable spot**: API サポートが利用可能な場合にスポットセクションを有効にします。
- **Demo** モード。
- **Spot REST / Derivatives REST** エンドポイント。
- **Spot WS / Derivatives public WS / Derivatives private WS** エンドポイント。

公式 API ドキュメント:

- [Authentication](https://edgex-1.gitbook.io/edgex-documentation/developer/api/authentication)
- [Order API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/order-api)
- [Account API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/account-api)
- [Private websocket stream](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/private-websocket-stream)
- [Funding API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/funding-api)
- [Meta-data API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/meta-data-api)
- [Quote API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/quote-api)

> [!TIP]
> `Derivatives` は完全に実装されています。`Spot` は対象の API 環境がサポートしている場合にのみ有効にしてください。
