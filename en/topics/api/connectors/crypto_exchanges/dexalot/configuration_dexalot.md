# Connector configuration: Dexalot

Configure the following properties before connecting to Dexalot. The list is verified against [DexalotMessageAdapter](xref:StockSharp.Dexalot.DexalotMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `RpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## Advanced settings

These properties control contract addresses, pair filters, depth, polling, and transaction behavior.

- `TradePairsAddress` (`string`)
- `PortfolioAddress` (`string`)
- `Pairs` (`string`)
- `OrderBookDepth` (`int`)
- `PrivatePollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `SelfTradePrevention` (`DexalotSelfTradePrevention`)

## See also

[Graphical configuration](graphical_configuration_dexalot.md)

[Adapter initialization](adapter_initialization_dexalot.md)
