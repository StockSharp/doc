# 连接器配置：Bigul

连接 Bigul 前，请配置以下适配器属性。该列表已根据 [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `ClientCode` (`string`)
- `ApiKey` (`SecureString`)
- `ApiSecret` (`SecureString`)
- `OneTimePassword` (`SecureString`)
- `Token` (`SecureString`)
- `Source` (`string`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `PortfolioName` (`string`)
- `DefaultProduct` (`BigulProducts`)
- `MarketProtection` (`decimal`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_bigul.md)

[适配器初始化](adapter_initialization_bigul.md)
