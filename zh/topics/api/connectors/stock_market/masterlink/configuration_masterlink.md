# 连接器配置：MasterLink

连接 MasterLink 前，请配置以下适配器属性。该列表已根据 [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Login` (`string`)
- `Password` (`SecureString`)
- `CertificatePath` (`string`)
- `CertificatePassword` (`SecureString`)
- `NodePath` (`string`)
- `GatewayDirectory` (`string`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `Account` (`string`)
- `RegisterApiAuth` (`bool`)
- `MarketDataMode` (`MasterLinkMarketDataModes`)
- `AdjustedCandles` (`bool`)
- `AccountPollingInterval` (`TimeSpan`)
- `MaxLookupResults` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_masterlink.md)

[适配器初始化](adapter_initialization_masterlink.md)
