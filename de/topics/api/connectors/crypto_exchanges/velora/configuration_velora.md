# Connector-Konfiguration: Velora

Konfigurieren Sie vor der Verbindung mit Velora die folgenden Adaptereigenschaften. Die Liste wurde anhand von [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Partner` (`string`)
- `Chain` (`VeloraChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Markets` (`string`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_velora.md)

[Adapterinitialisierung](adapter_initialization_velora.md)
