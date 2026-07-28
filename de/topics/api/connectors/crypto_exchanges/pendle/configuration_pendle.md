# Connector-Konfiguration: Pendle

Konfigurieren Sie vor der Verbindung mit Pendle die folgenden Adaptereigenschaften. Die Liste wurde anhand von [PendleMessageAdapter](xref:StockSharp.Pendle.PendleMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Chain` (`PendleChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Marktauswahl, Grenzwerte, Abfragen und Transaktionsverhalten.

- `MarketAddresses` (`string`)
- `MaxMarkets` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `HistoryLimit` (`int`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_pendle.md)

[Adapterinitialisierung](adapter_initialization_pendle.md)
