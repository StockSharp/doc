# 连接器配置：FXOpen TickTrader

创建 FXOpen Web API 令牌并填写连接参数。

- `WebApiId` - Web API 令牌标识符。
- `Key` - Web API 密钥。
- `Secret` - Web API 密钥密码。
- `OneTimePassword` - 账户需要双重身份验证时使用的可选一次性密码。
- `IsDemo` - 选择模拟环境。默认值为 `false`。
- `Address` - REST 端点。实盘默认值为 `https://ttlivewebapi.fxopen.net`。
- `FeedAddress` - Feed WebSocket。实盘默认值为 `wss://marginalttlivewebapi.fxopen.net/feed`。
- `TradeAddress` - Trade WebSocket。实盘默认值为 `wss://marginalttlivewebapi.fxopen.net/trade`。

启用 `IsDemo` 时，如果地址未被手动修改，将使用官方 TickTrader 模拟端点。WebSocket 订阅和所有私有操作都需要 ID、密钥和密钥密码。

## 另请参阅

[FXOpen 官方 API 文档](https://ticktrader.fxopen.com/api)

[TickTrader Web REST API](https://ttlivewebapi.fxopen.net/api/doc/index?apiaddress=ttlivewebapi.fxopen.net&apiport=443)
