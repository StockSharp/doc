# 连接器配置：XRPL DEX

连接 XRPL DEX 前，请配置以下适配器属性。该列表已根据 [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `RpcEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `Account` (`string`)
- `Seed` (`SecureString`)

## 高级设置

这些属性用于控制市场选择、订单簿深度、历史数据、费用、轮询和交易保护。

- `Markets` (`string`)
- `DomainId` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLedgerLimit` (`int`)
- `FeeMultiplier` (`decimal`)
- `LastLedgerOffset` (`int`)
- `MarketOrderProtection` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_xrpl.md)

[适配器初始化](adapter_initialization_xrpl.md)
