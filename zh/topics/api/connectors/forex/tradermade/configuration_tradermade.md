# 连接器配置：TraderMade

连接 TraderMade 前，请配置以下适配器属性。该列表已根据 [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `RestKey` (`SecureString`)

## 高级设置

这些属性用于控制 REST 与 WebSocket 访问、服务端点、市场数据选项、交易对筛选和结果限制。

- `StreamingKey` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `EnableLadder` (`bool`)
- `Weekend` (`bool`)
- `QuoteCurrencies` (`string`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_tradermade.md)

[适配器初始化](adapter_initialization_tradermade.md)
