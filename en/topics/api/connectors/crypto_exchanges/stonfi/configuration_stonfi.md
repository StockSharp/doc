# Connector configuration: STON.fi

Configure the following properties before connecting to STON.fi. The list is verified against [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `ApiEndpoint` (`string`)
- `TonCenterEndpoint` (`string`)
- `TonCenterApiKey` (`SecureString`)
- `WalletAddress` (`string`)
- `Mnemonic` (`SecureString`)

## Advanced settings

These properties control wallet details, pool selection, limits, polling, history, and transaction behavior.

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

## See also

[Graphical configuration](graphical_configuration_stonfi.md)

[Adapter initialization](adapter_initialization_stonfi.md)
