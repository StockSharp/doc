# 连接器配置：TASE Data Hub

连接 TASE Data Hub 前，请配置以下适配器属性。该列表已根据 [TaseDataHubMessageAdapter](xref:StockSharp.TaseDataHub.TaseDataHubMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `Address` (`Uri`)
- `Scope` (`string`)
- `SecurityLookupDays` (`int`)
- `ReferenceCacheTimeout` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_tase_data_hub.md)

[适配器初始化](adapter_initialization_tase_data_hub.md)
