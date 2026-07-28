# 连接器配置：LCX

连接 LCX 前，请配置以下适配器属性。该列表已根据 [LcxMessageAdapter](xref:StockSharp.LCX.LcxMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RestEndpoint` (`string`)
- `KlineEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `ApiVersion` (`string`)
- `PrivatePollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_lcx.md)

[适配器初始化](adapter_initialization_lcx.md)
