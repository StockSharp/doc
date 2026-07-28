# 连接器配置：Velodrome

连接 Velodrome 前，请配置以下适配器属性。该列表已根据 [VelodromeMessageAdapter](xref:StockSharp.Velodrome.VelodromeMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `RpcEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `Pools` (`string`)
- `HistoryBlockRange` (`int`)
- `HistoryBlockCount` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_velodrome.md)

[适配器初始化](adapter_initialization_velodrome.md)
