# 连接器配置：StocksTrader

在 StocksTrader 网页终端中生成令牌并填写连接参数。

- `Token` - 网页终端签发的 Bearer 令牌。
- `AccountId` - 账户标识符。当所选模式下恰好只有一个账户时可以留空。
- `IsDemo` - 选择模拟账户。默认值为 `true`。
- `Address` - REST 端点。默认值为 `https://api.stockstrader.com/`。
- `PollingInterval` - 请求订单、成交和账户状态的频率。默认值为 5 秒，更短的值会被提升到 2 秒。

由于该服务商不提供流式推送，`PollingInterval` 决定了订单和持仓变化传达到策略的速度。活跃交易时可以缩短该值，若要遵守服务商的请求限制则可以延长该值。

保护性价格通过 [StocksTraderOrderCondition](xref:StockSharp.StocksTrader.StocksTraderOrderCondition) 传递：订单或未平仓持仓的止损和止盈价格。

## 另请参阅

[StocksTrader 官方 API 文档](https://api-doc.stockstrader.com/)
