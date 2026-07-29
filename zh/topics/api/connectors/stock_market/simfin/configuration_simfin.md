# 连接器配置：SimFin

连接 SimFin 前，请配置以下适配器属性。该列表已根据 [SimFinMessageAdapter](xref:StockSharp.SimFin.SimFinMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)

## 高级设置

这些属性用于控制服务端点、请求间隔、筛选条件、数据选项和结果限制。

- `RestEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `StatementTypes` (`string`)
- `Period` (`string`)
- `AsReported` (`bool`)
- `IncludeRatios` (`bool`)
- `MaximumRecords` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_simfin.md)

[适配器初始化](adapter_initialization_simfin.md)
