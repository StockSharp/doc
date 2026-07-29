# Connector-Konfiguration: SSI

Konfigurieren Sie vor der Verbindung mit SSI die folgenden Adaptereigenschaften. Die Liste wurde anhand von [SSIMessageAdapter](xref:StockSharp.SSI.SSIMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ClientId` (`string`)
- `Account` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Authentifizierung und Sitzungszustand, Anbieterendpunkte, Echtzeitübertragung und Abfrageintervalle.

- `PrivateKey` (`SecureString`)
- `Otp` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_ssi.md)

[Adapterinitialisierung](adapter_initialization_ssi.md)
