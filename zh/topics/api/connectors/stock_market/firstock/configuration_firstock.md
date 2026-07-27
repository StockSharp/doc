# 连接器配置：Firstock

连接 Firstock 前，请配置以下适配器属性。该列表已根据 [FirstockMessageAdapter](xref:StockSharp.Firstock.FirstockMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `UserId` (`string`)
- `Password` (`SecureString`)
- `OneTimePassword` (`SecureString`)
- `VendorCode` (`string`)
- `ApiKey` (`SecureString`)
- `Token` (`SecureString`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `PortfolioName` (`string`)
- `DefaultProduct` (`FirstockProducts`)
- `MarketProtection` (`decimal`)
- `PriceDivisor` (`decimal`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `SymbolsAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_firstock.md)

[适配器初始化](adapter_initialization_firstock.md)
