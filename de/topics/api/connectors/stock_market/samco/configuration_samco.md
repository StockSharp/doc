# Connector-Konfiguration: Samco

Konfigurieren Sie vor der Verbindung mit Samco die folgenden Adaptereigenschaften. Die Liste wurde anhand von [SamcoMessageAdapter](xref:StockSharp.Samco.SamcoMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Authentifizierung und Sitzungszustand, Anbieterendpunkte, Echtzeitübertragung und Abfrageintervalle.

- `Secret` (`SecureString`)
- `SessionToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `InstrumentEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_samco.md)

[Adapterinitialisierung](adapter_initialization_samco.md)
