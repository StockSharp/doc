# 连接器配置：Choice FinX

连接 Choice FinX 前，请配置以下适配器属性。该列表已根据 [ChoiceFinXMessageAdapter](xref:StockSharp.ChoiceFinX.ChoiceFinXMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Token` (`SecureString`)
- `AuthorizationHeader` (`string`)
- `AuthorizationScheme` (`string`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `VendorId` (`string`)
- `VendorKey` (`SecureString`)
- `WebSocketToken` (`SecureString`)
- `DefaultProduct` (`ChoiceFinXProducts`)
- `PortfolioName` (`string`)
- `ModeType` (`string`)
- `Mode` (`int?`)
- `DeviceId` (`string`)
- `PriceDivisor` (`decimal`)
- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_choice_finx.md)

[适配器初始化](adapter_initialization_choice_finx.md)
