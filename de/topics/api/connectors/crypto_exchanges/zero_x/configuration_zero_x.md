# Connector-Konfiguration: 0x

Konfigurieren Sie vor der Verbindung mit 0x die folgenden Adaptereigenschaften. Die Liste wurde anhand von [ZeroXMessageAdapter](xref:StockSharp.ZeroX.ZeroXMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `ApiKey` (`SecureString`)
- `Chain` (`ZeroXChains`)
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

[Grafische Konfiguration](graphical_configuration_zero_x.md)

[Adapterinitialisierung](adapter_initialization_zero_x.md)
