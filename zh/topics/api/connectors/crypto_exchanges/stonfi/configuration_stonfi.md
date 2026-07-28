# 连接器配置：STON.fi

连接 STON.fi 前，请配置以下适配器属性。该列表已根据 [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `ApiEndpoint` (`string`)
- `TonCenterEndpoint` (`string`)
- `TonCenterApiKey` (`SecureString`)
- `WalletAddress` (`string`)
- `Mnemonic` (`SecureString`)

## 高级设置

这些属性用于控制钱包信息、流动性池选择、限制、轮询、历史数据和交易行为。

- `WalletSubwalletId` (`uint`)
- `WalletRevision` (`int`)
- `Pools` (`string`)
- `PoolLimit` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `HistoryBlockLimit` (`int`)
- `PrivatePollingInterval` (`TimeSpan`)
- `TransactionTimeout` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_stonfi.md)

[适配器初始化](adapter_initialization_stonfi.md)
