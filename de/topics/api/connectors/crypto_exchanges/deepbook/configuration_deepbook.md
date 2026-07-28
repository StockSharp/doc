# Connector-Konfiguration: DeepBook

Konfigurieren Sie vor der Verbindung mit DeepBook die folgenden Adaptereigenschaften. Die Liste wurde anhand von [DeepBookMessageAdapter](xref:StockSharp.DeepBook.DeepBookMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `IndexerEndpoint` (`string`)
- `GrpcEndpoint` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `PackageId` (`string`)
- `ClockObjectId` (`string`)
- `Pools` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLimit` (`int`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_deepbook.md)

[Adapterinitialisierung](adapter_initialization_deepbook.md)
