# Paradex の設定

コネクタを使用するには、API 認証情報と Starknet 認証設定を指定します。

主な設定:

- **キー** と **シークレット**。
- **Starknet account** と **Starknet key**。
- **Section**: `Spot` または `Derivatives`。
- **Enable spot**: API サポートが利用可能な場合に Spot セクションを有効にします。
- **Demo** モード。
- **Spot REST / Derivatives REST** エンドポイント。
- **Spot WS / Derivatives WS** エンドポイント。
- **Auth path** (デフォルト: `/v1/auth`)。

公式 API ドキュメント:

- [API URL](https://docs.paradex.trade/api/prod/api-urls)
- [認証](https://docs.paradex.trade/api/prod/authentication)
- [REST API](https://docs.paradex.trade/api/prod/rest-api)
- [新規注文の作成](https://docs.paradex.trade/api/prod/orders/create-a-new-order)
- [WebSocket の概要](https://docs.paradex.trade/api/prod/websocket/introduction)
- [WebSocket チャネル](https://docs.paradex.trade/api/prod/websocket/channels)
- [オーダーブックチャネル](https://docs.paradex.trade/api/prod/websocket/channels/order_book_channel)

> [!TIP]
> Paradex のデリバティブは完全にサポートされています。対象 API 環境で Spot サポートが確認されている場合にのみ `Spot` を有効にしてください。
