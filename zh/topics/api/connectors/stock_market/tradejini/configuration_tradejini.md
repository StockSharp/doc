# 连接器配置：Tradejini

连接 Tradejini 前，请配置以下适配器属性。该列表已根据 [TradejiniMessageAdapter](xref:StockSharp.Tradejini.TradejiniMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `ApiKey` (`SecureString`)
- `Password` (`SecureString`)
- `TwoFactorCode` (`SecureString`)
- `TwoFactorType` (`TradejiniTwoFactorTypes`)
- `Token` (`SecureString`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `PortfolioName` (`string`)
- `DefaultProduct` (`TradejiniProducts`)
- `Address` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_tradejini.md)

[适配器初始化](adapter_initialization_tradejini.md)
