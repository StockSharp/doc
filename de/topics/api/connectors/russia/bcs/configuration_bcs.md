# Connector-Konfiguration: BCS

Konfigurieren Sie vor der Verbindung mit BCS die folgenden Adaptereigenschaften. Die Liste wurde anhand von [BcsMessageAdapter](xref:StockSharp.Bcs.BcsMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `IsReadOnly` (`bool`)
- `PortfolioName` (`string`)
- `PollingInterval` (`TimeSpan`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_bcs.md)

[Adapterinitialisierung](adapter_initialization_bcs.md)
