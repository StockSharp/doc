# 连接器配置：DNSE

连接 DNSE 前，请配置以下适配器属性。该列表已根据 [DnseMessageAdapter](xref:StockSharp.Dnse.DnseMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `TradingToken` (`SecureString`)
- `Account` (`string`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `OtpType` (`DnseOtpTypes`)
- `OneTimePassword` (`SecureString`)
- `RequestEmailOtpOnConnect` (`bool`)
- `DefaultLoanPackageId` (`int`)
- `DefaultBoardId` (`string`)
- `MarketDataPriceMultiplier` (`decimal`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `ApiVersion` (`string`)
- `DateHeaderName` (`string`)
- `RestAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)

## 另请参阅

[图形化配置](graphical_configuration_dnse.md)

[适配器初始化](adapter_initialization_dnse.md)
