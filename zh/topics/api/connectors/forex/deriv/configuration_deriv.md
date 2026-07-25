# 连接器配置：Deriv

创建 Deriv 令牌并填写连接参数。

- `Token` - 个人访问令牌或 OAuth 令牌。私有操作必需。
- `AppId` - 随已验证的 REST 请求一起发送的应用程序标识符。
- `AccountId` - 期权账户标识符。当所选模式下恰好只有一个活跃账户时可以留空。
- `IsDemo` - 选择模拟账户。默认值为 `true`。
- `RestAddress` - REST 端点。默认值为 `https://api.derivws.com`。
- `PublicWebSocketAddress` - 公共期权 WebSocket 端点。默认值为 `wss://api.derivws.com/trading/v1/options/ws/public`。

公共市场数据会话无需令牌即可运行，而合约、余额和交易流水则需要令牌和应用程序 ID。订阅会通过新的一次性 WebSocket 地址自动恢复。

合约参数通过 [DerivOrderCondition](xref:StockSharp.Deriv.DerivOrderCondition) 传递：合约类型、金额是投注额还是赔付额、合约货币、期限、障碍价位，以及保护性止损和止盈价格。

## 另请参阅

[Deriv 官方 API 文档](https://developers.deriv.com/docs/)
