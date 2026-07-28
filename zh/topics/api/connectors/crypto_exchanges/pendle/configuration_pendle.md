# 连接器配置：Pendle

连接 Pendle 前，请配置以下适配器属性。该列表已根据 [PendleMessageAdapter](xref:StockSharp.Pendle.PendleMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Chain` (`PendleChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## 高级设置

这些属性用于控制市场选择、限制、轮询和交易行为。

- `MarketAddresses` (`string`)
- `MaxMarkets` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `HistoryLimit` (`int`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## 另请参阅

[图形化配置](graphical_configuration_pendle.md)

[适配器初始化](adapter_initialization_pendle.md)
