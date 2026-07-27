# 连接器配置：Nuvama

连接 Nuvama 前，请配置以下适配器属性。该列表已根据 [NuvamaMessageAdapter](xref:StockSharp.Nuvama.NuvamaMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RequestId` (`SecureString`)
- `AppIdKey` (`SecureString`)
- `PublicIpAddress` (`string`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `VendorToken` (`SecureString`)
- `Token` (`SecureString`)
- `AccountId` (`string`)
- `UserId` (`string`)
- `AccountType` (`string`)
- `EmployeeOrDependent` (`string`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`NuvamaProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`Uri`)
- `IpAddressService` (`Uri`)
- `StreamHost` (`string`)
- `StreamPort` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_nuvama.md)

[适配器初始化](adapter_initialization_nuvama.md)
