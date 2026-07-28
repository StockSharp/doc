# 连接器配置：Coincall

连接 Coincall 前，请配置以下适配器属性。该列表已根据 [CoincallMessageAdapter](xref:StockSharp.Coincall.CoincallMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ProductType` (`CoincallProductTypes`)
- `RestEndpoint` (`string`)
- `OptionsWebSocketEndpoint` (`string`)
- `FuturesWebSocketEndpoint` (`string`)
- `RequestValidityWindow` (`TimeSpan`)
- `PrivatePollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_coincall.md)

[适配器初始化](adapter_initialization_coincall.md)
