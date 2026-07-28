# 连接器配置：Velora

连接 Velora 前，请配置以下适配器属性。该列表已根据 [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Partner` (`string`)
- `Chain` (`VeloraChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `Markets` (`string`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## 另请参阅

[图形化配置](graphical_configuration_velora.md)

[适配器初始化](adapter_initialization_velora.md)
