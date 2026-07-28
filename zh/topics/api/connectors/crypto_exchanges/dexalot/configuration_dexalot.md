# 连接器配置：Dexalot

连接 Dexalot 前，请配置以下适配器属性。该列表已根据 [DexalotMessageAdapter](xref:StockSharp.Dexalot.DexalotMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `RpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## 高级设置

这些属性用于控制合约地址、交易对筛选、订单簿深度、轮询和交易行为。

- `TradePairsAddress` (`string`)
- `PortfolioAddress` (`string`)
- `Pairs` (`string`)
- `OrderBookDepth` (`int`)
- `PrivatePollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `SelfTradePrevention` (`DexalotSelfTradePrevention`)

## 另请参阅

[图形化配置](graphical_configuration_dexalot.md)

[适配器初始化](adapter_initialization_dexalot.md)
