# 连接器配置：InvertirOnline

连接 InvertirOnline 前，请配置以下适配器属性。该列表已根据 [InvertirOnlineMessageAdapter](xref:StockSharp.InvertirOnline.InvertirOnlineMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Login` (`string`)
- `Password` (`SecureString`)
- `IsDemo` (`bool`)
- `PortfolioName` (`string`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `Token` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `DefaultCountry` (`InvertirOnlineCountries`)
- `DefaultMarket` (`string`)
- `DefaultInstrumentType` (`string`)
- `DefaultSettlement` (`InvertirOnlineSettlements`)
- `AdjustedHistory` (`bool`)
- `MarketDataPollingInterval` (`TimeSpan`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)

## 另请参阅

[图形化配置](graphical_configuration_invertironline.md)

[适配器初始化](adapter_initialization_invertironline.md)
