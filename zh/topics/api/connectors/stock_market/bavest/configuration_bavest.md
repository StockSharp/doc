# 连接器配置：Bavest

连接 Bavest 前，请配置以下适配器属性。该列表已根据 [BavestMessageAdapter](xref:StockSharp.Bavest.BavestMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Token` (`SecureString`)
- `Address` (`Uri`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `Currency` (`string`)
- `Exchange` (`string`)
- `ExchangeCode` (`string`)
- `FinancialFrequency` (`BavestFinancialFrequencies`)
- `TraceEtfMetrics` (`bool`)
- `ScreenerQuery` (`string`)
- `PageSize` (`int`)
- `MaxPages` (`int`)
- `NewsLimit` (`int`)
- `DatasetLimit` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_bavest.md)

[适配器初始化](adapter_initialization_bavest.md)
