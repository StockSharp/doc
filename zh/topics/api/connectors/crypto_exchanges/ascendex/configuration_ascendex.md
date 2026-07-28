# 连接器配置：AscendEX

连接 AscendEX 前，请配置以下适配器属性。该列表已根据 [AscendExMessageAdapter](xref:StockSharp.AscendEx.AscendExMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AccountGroup` (`int`)
- `SpotAccountType` (`AscendExSpotAccountTypes`)
- `RestEndpoint` (`string`)
- `SpotWebSocketEndpoint` (`string`)
- `FuturesWebSocketEndpoint` (`string`)

## 另请参阅

[图形化配置](graphical_configuration_ascendex.md)

[适配器初始化](adapter_initialization_ascendex.md)
