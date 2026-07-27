# 连接器配置：FINRA

连接 FINRA 前，请配置以下适配器属性。该列表已根据 [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`FinraDataSets`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `Token` (`SecureString`)
- `WeeklyTierIdentifier` (`string`)
- `WeeklySummaryTypeCode` (`string`)
- `PageSize` (`int`)
- `MaxRecords` (`int`)
- `DataVersion` (`int`)
- `Address` (`Uri`)
- `AuthAddress` (`Uri`)

## 另请参阅

[图形化配置](graphical_configuration_finra.md)

[适配器初始化](adapter_initialization_finra.md)
