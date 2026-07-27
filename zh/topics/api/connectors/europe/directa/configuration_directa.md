# 连接器配置：Directa

连接 Directa 前，请配置以下适配器属性。该列表已根据 [DirectaMessageAdapter](xref:StockSharp.Directa.DirectaMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Address` (`EndPoint`)
- `DataAddress` (`EndPoint`)
- `HistoryAddress` (`EndPoint`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `RequestTimeout` (`TimeSpan`)
- `AutoConfirmOrders` (`bool`)
- `MaxMarketDepth` (`int`)
- `TimeZoneId` (`string`)

## 另请参阅

[图形化配置](graphical_configuration_directa.md)

[适配器初始化](adapter_initialization_directa.md)
