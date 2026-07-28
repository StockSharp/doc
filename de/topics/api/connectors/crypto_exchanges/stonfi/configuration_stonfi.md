# Connector-Konfiguration: STON.fi

Konfigurieren Sie vor der Verbindung mit STON.fi die folgenden Adaptereigenschaften. Die Liste wurde anhand von [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `ApiEndpoint` (`string`)
- `TonCenterEndpoint` (`string`)
- `TonCenterApiKey` (`SecureString`)
- `WalletAddress` (`string`)
- `Mnemonic` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Wallet-Details, Poolauswahl, Grenzwerte, Abfragen, Historie und Transaktionsverhalten.

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

## Siehe auch

[Grafische Konfiguration](graphical_configuration_stonfi.md)

[Adapterinitialisierung](adapter_initialization_stonfi.md)
