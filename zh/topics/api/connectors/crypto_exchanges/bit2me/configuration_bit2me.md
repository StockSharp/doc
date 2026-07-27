# Bit2Me 连接器设置

公开市场数据无需凭据。账户和交易操作需要配置 API 密钥和 Secret。

## 连接参数

- `Key` — Bit2Me API 密钥。
- `Secret` — Bit2Me API Secret。
- `RestEndpoint` — REST API 地址。生产环境默认值为 `https://gateway.bit2me.com`。
- `WebSocketEndpoint` — WebSocket 地址。生产环境默认值为 `wss://ws.bit2me.com/v1/trading`。

连接器支持市价单、限价单和止损限价单。公开 WebSocket 订阅提供成交和完整的 Level 2 订单簿更新；K 线通过 REST 下载。

## 官方 API 文档

- [Bit2Me API](https://api.bit2me.com/)
- [Bit2Me 交易示例](https://github.com/bit2me-devs/trading-spot-samples)
