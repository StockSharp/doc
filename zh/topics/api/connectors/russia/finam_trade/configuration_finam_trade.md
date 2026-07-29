# 连接器配置：Finam Trade API

连接 Finam 前，请配置以下属性。该列表已根据 [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Token` (`SecureString`) — 必填的 Finam Trade API 密钥。适配器会将其换取短期会话令牌。
- `AccountId` (`string`) — 可选的交易账户标识符。留空时，适配器使用令牌可访问的第一个账户。

## 高级设置

- `AppId` (`string`) — 创建会话时发送的应用程序标识符。默认值为 `StockSharp`。
- `PollingInterval` (`TimeSpan`) — 轮询账户和订单快照的时间间隔。默认值为 30 秒；小于一秒的值无效。
- `LookupLimit` (`int`) — 无限制查询最多返回的交易品种数量。默认值为 `10000`，且必须为正数。
- `RestAddress` (`string`) — REST API 基础地址。默认值为 `https://api.finam.ru/`。
- `WebSocketAddress` (`string`) — WebSocket API 地址。默认值为 `wss://api.finam.ru/ws`。

除非 Finam 或兼容网关提供了其他地址，否则请保留预填的地址。

## 另请参阅

[图形化配置](graphical_configuration_finam_trade.md)

[适配器初始化](adapter_initialization_finam_trade.md)
