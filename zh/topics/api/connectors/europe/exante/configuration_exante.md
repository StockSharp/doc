# 连接器配置：EXANTE

连接 EXANTE 前，请配置以下适配器属性。该列表已根据 [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `SummaryCurrency` (`string`)
- `PollingInterval` (`TimeSpan`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `MaxMarketDepth` (`int`)
- `HistoryRequestSize` (`int`)
- `LiveAddress` (`Uri`)
- `DemoAddress` (`Uri`)

## 另请参阅

[图形化配置](graphical_configuration_exante.md)

[适配器初始化](adapter_initialization_exante.md)
