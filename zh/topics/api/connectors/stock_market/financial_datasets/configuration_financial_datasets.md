# 连接器配置：Financial Datasets

连接 Financial Datasets 前，请配置以下适配器属性。该列表已根据 [FinancialDatasetsMessageAdapter](xref:StockSharp.FinancialDatasets.FinancialDatasetsMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Token` (`SecureString`)
- `Address` (`Uri`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `ActiveOnly` (`bool`)
- `FinancialPeriod` (`FinancialDatasetsPeriods`)
- `DataLimit` (`int`)
- `NewsLimit` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_financial_datasets.md)

[适配器初始化](adapter_initialization_financial_datasets.md)
