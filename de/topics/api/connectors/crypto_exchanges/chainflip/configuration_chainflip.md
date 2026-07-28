# Connector-Konfiguration: Chainflip

Konfigurieren Sie vor der Verbindung mit Chainflip die folgenden Adaptereigenschaften. Die Liste wurde anhand von [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `StateRpcEndpoint` (`string`)
- `BackendEndpoint` (`string`)
- `EthereumRpcEndpoint` (`string`)
- `ArbitrumRpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Zieladressen, Poolfilter, Abfragen, Auftragsbuchtiefe und Transaktionsverhalten.

- `BitcoinAddress` (`string`)
- `SolanaAddress` (`string`)
- `AssethubAddress` (`string`)
- `PolkadotAddress` (`string`)
- `TronAddress` (`string`)
- `Pools` (`string`)
- `ProbeVolume` (`decimal`)
- `OrderBookDepth` (`int`)
- `PollingInterval` (`TimeSpan`)
- `MaxBlocksPerPoll` (`int`)
- `InitialTickBlocks` (`int`)
- `SlippageTolerance` (`decimal`)
- `RetryDurationBlocks` (`int`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_chainflip.md)

[Adapterinitialisierung](adapter_initialization_chainflip.md)
