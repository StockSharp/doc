# 连接器配置：Samco

连接 Samco 前，请配置以下适配器属性。该列表已根据 [SamcoMessageAdapter](xref:StockSharp.Samco.SamcoMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)

## 高级设置

这些属性用于控制身份验证和会话状态、服务端点、流式数据及轮询。

- `Secret` (`SecureString`)
- `SessionToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `InstrumentEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_samco.md)

[适配器初始化](adapter_initialization_samco.md)
