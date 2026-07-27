# 连接器配置：Open DART

连接 Open DART 前，请配置以下适配器属性。该列表已根据 [OpenDartMessageAdapter](xref:StockSharp.OpenDart.OpenDartMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Token` (`SecureString`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `Address` (`Uri`)
- `DisclosureAddress` (`Uri`)
- `DisclosureType` (`OpenDartDisclosureTypes`)
- `CorporationClass` (`OpenDartCorporationClasses`)
- `FinalReportsOnly` (`bool`)
- `BusinessYear` (`int?`)
- `ReportType` (`OpenDartReportTypes`)
- `FinancialSearchYears` (`int`)
- `MaxPages` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_open_dart.md)

[适配器初始化](adapter_initialization_open_dart.md)
