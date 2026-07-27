# 连接器配置：Tradernet

连接 Tradernet 前，请配置以下适配器属性。该列表已根据 [TradernetMessageAdapter](xref:StockSharp.Tradernet.TradernetMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `PollingInterval` (`TimeSpan`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `MaxMarketDepth` (`int`)
- `SecuritiesPageSize` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_tradernet.md)

[适配器初始化](adapter_initialization_tradernet.md)
