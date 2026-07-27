# 连接器配置：KRX Open API

连接 KRX Open API 前，请配置以下适配器属性。该列表已根据 [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Token` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`KrxDataSets`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `MaxRequests` (`int`)
- `Address` (`Uri`)
- `SampleAddress` (`Uri`)

## 另请参阅

[图形化配置](graphical_configuration_krx_open_api.md)

[适配器初始化](adapter_initialization_krx_open_api.md)
