# 连接器配置：WazirX

连接 WazirX 前，请配置以下适配器属性。该列表已根据 [WazirXMessageAdapter](xref:StockSharp.WazirX.WazirXMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `ReceiveWindow` (`long`)
- `PrivatePollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_wazirx.md)

[适配器初始化](adapter_initialization_wazirx.md)
