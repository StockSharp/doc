# 连接器配置：m.Stock

连接 m.Stock 前，请配置以下适配器属性。该列表已根据 [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `ClientCode` (`string`)

## 高级设置

这些属性用于控制身份验证和会话状态、服务端点、流式数据及轮询。

- `Password` (`SecureString`)
- `Otp` (`SecureString`)
- `UseTotp` (`bool`)
- `RefreshToken` (`SecureString`)
- `AccessToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_mstock.md)

[适配器初始化](adapter_initialization_mstock.md)
