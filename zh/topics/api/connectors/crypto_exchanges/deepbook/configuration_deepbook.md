# 连接器配置：DeepBook

连接 DeepBook 前，请配置以下适配器属性。该列表已根据 [DeepBookMessageAdapter](xref:StockSharp.DeepBook.DeepBookMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `IndexerEndpoint` (`string`)
- `GrpcEndpoint` (`string`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `PackageId` (`string`)
- `ClockObjectId` (`string`)
- `Pools` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLimit` (`int`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_deepbook.md)

[适配器初始化](adapter_initialization_deepbook.md)
