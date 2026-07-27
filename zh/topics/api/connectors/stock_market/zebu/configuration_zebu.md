# 连接器配置：Zebu

连接 Zebu 前，请配置以下适配器属性。该列表已根据 [ZebuMessageAdapter](xref:StockSharp.Zebu.ZebuMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AuthorizationCode` (`SecureString`)
- `UserId` (`string`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `RefreshToken` (`SecureString`)
- `Token` (`SecureString`)
- `AccountId` (`string`)
- `TokenExpiresAt` (`DateTime?`)
- `DefaultProduct` (`ShoonyaProducts`)
- `ReconnectAttempts` (`int`)
- `AuthorizationAddress` (`Uri`)
- `RestEndpoint` (`string`)
- `InstrumentEndpointTemplate` (`string`)
- `WebSocketEndpoint` (`string`)

## 另请参阅

[图形化配置](graphical_configuration_zebu.md)

[适配器初始化](adapter_initialization_zebu.md)
