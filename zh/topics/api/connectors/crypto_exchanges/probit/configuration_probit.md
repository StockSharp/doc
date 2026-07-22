# ProBit Global 连接器设置

请在连接器设置中指定 ProBit Global 的连接参数。

## 连接参数

- `Key` - ProBit API 凭据中签发的 OAuth 客户端标识符。
- `Secret` - OAuth 客户端密钥。
- `RestEndpoint` - REST API 地址。
- `AuthEndpoint` - OAuth 令牌端点地址。
- `WebSocketEndpoint` - WebSocket 服务器地址。

公共市场数据无需凭据。交易、余额、订单历史和私有 WebSocket 频道需要 `Key` 和 `Secret`。

对于市价买单，请在 `ProBitOrderCondition.QuoteAmount` 中设置计价货币金额。

## 官方 API 文档

- [ProBit Global API 文档](https://docs-en.probit.com/)
- [ProBit Global API 凭据](https://www.probit.com/en-us/my-page/api-management/api-credential)
