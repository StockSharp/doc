# 连接器配置：TWSE

连接 TWSE 前，请配置以下适配器属性。该列表已根据 [TwseMessageAdapter](xref:StockSharp.Twse.TwseMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Address` (`Uri`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `IncludeProfiles` (`bool`)
- `IncludeValuations` (`bool`)
- `CacheTimeout` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_twse_openapi.md)

[适配器初始化](adapter_initialization_twse_openapi.md)
