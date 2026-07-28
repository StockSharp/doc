# 连接器配置：Settrade

连接 Settrade 前，请配置以下适配器属性。该列表已根据 [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AppCode` (`string`)
- `BrokerId` (`string`)
- `Account` (`string`)
- `Pin` (`SecureString`)
- `AccountType` (`SettradeAccountTypes`)
- `IsDemo` (`bool`)

## 高级设置

这些属性用于控制登录参数、生产和测试端点，以及私有状态轮询。

- `LoginParameters` (`string`)
- `RestEndpoint` (`string`)
- `DemoRestEndpoint` (`string`)
- `MarketDataEndpoint` (`string`)
- `DemoMarketDataEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_settrade.md)

[适配器初始化](adapter_initialization_settrade.md)
