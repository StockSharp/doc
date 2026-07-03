# Paradex 配置

要使用连接器，请指定您的 API 凭据和 Starknet 身份验证设置。

主要设置：

- **Key** 和 **Secret**。
- **Starknet 账户** 和 **Starknet 密钥**。
- **部分**：`Spot` 或 `Derivatives`。
- **启用现货**：当 API 支持可用时启用现货部分。
- **演示** 模式。
- **现货 REST / 衍生品 REST** 端点。
- **现货 WS / 衍生品 WS** 端点。
- **认证路径**（默认值：`/v1/auth`）。

官方 API 文档：

- [API URLs](https://docs.paradex.trade/api/prod/api-urls)
- [身份验证](https://docs.paradex.trade/api/prod/authentication)
- [REST API](https://docs.paradex.trade/api/prod/rest-api)
- [创建新订单](https://docs.paradex.trade/api/prod/orders/create-a-new-order)
- [Websocket 介绍](https://docs.paradex.trade/api/prod/websocket/introduction)
- [Websocket 通道](https://docs.paradex.trade/api/prod/websocket/channels)
- [订单簿频道](https://docs.paradex.trade/api/prod/websocket/channels/order_book_channel)

> [!TIP]
> Paradex 衍生品得到完全支持。仅当目标 API 环境确认现货支持时，才启用 `Spot`。
