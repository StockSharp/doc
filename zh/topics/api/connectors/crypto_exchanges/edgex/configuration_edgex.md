# edgeX 配置

要使用连接器，请在交易所账户中生成 **API 密钥** 和 **密钥**，并在连接设置中指定它们。

主要设置：

- **访问密钥** 和 **密钥**。
- **清算账户** 和 **口令**。
- **交易分区**：`现货` 或 `衍生品`。
- **启用现货**：当 API 支持可用时启用现货板块。
- **模拟模式** 模式。
- **现货 REST / 衍生品 REST** 端点。
- **现货 WS / 衍生品公共 WS / 衍生品私有 WS** 端点。

官方 API 文档：

- [身份验证](https://edgex-1.gitbook.io/edgex-documentation/developer/api/authentication)
- [订单 API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/order-api)
- [账户 API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/account-api)
- [私有 WebSocket 流](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/private-websocket-stream)
- [资金 API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/funding-api)
- [元数据 API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/meta-data-api)
- [引用 API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/quote-api)

> [!TIP]
> `衍生品` 已完全实现。只有在目标 API 环境支持时，才应启用 `现货`。
