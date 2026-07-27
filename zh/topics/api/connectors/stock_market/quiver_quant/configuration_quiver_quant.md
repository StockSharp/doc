# 连接器配置：Quiver Quantitative

连接 Quiver Quantitative 前，请配置以下适配器属性。该列表已根据 [QuiverQuantMessageAdapter](xref:StockSharp.QuiverQuant.QuiverQuantMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Token` (`SecureString`)
- `Address` (`Uri`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `PageSize` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `NewsLimit` (`int`)
- `LimitInsiderCodes` (`bool`)
- `MostRecentInstitutional` (`bool`)
- `IncludeNewFunds` (`bool`)
- `CorporateDonorCycle` (`string`)

## 另请参阅

[图形化配置](graphical_configuration_quiver_quant.md)

[适配器初始化](adapter_initialization_quiver_quant.md)
