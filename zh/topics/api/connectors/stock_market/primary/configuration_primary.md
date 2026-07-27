# 连接器配置：Primary

连接 Primary 前，请配置以下适配器属性。该列表已根据 [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Login` (`string`)
- `Password` (`SecureString`)
- `IsDemo` (`bool`)
- `Account` (`string`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `Token` (`SecureString`)
- `Proprietary` (`string`)
- `DefaultMarket` (`string`)
- `MarketDataLevel` (`int`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `SandboxWebSocketAddress` (`Uri`)

## 另请参阅

[图形化配置](graphical_configuration_primary.md)

[适配器初始化](adapter_initialization_primary.md)
