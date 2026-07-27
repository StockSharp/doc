# 连接器配置：SET Market Data

连接 SET Market Data 前，请配置以下适配器属性。该列表已根据 [SetMarketDataMessageAdapter](xref:StockSharp.SetMarketData.SetMarketDataMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Token` (`SecureString`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `Address` (`Uri`)
- `DataMode` (`SetMarketDataModes`)
- `Markets` (`string`)
- `IndexSectors` (`string`)
- `SecurityTypeCodes` (`string`)
- `IncludeOddLots` (`bool`)
- `IncludeIndices` (`bool`)

## 另请参阅

[图形化配置](graphical_configuration_set_market_data.md)

[适配器初始化](adapter_initialization_set_market_data.md)
