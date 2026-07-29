# Connector-Konfiguration: IIFL

Konfigurieren Sie vor der Verbindung mit IIFL die folgenden Adaptereigenschaften. Die Liste wurde anhand von [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ClientId` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Authentifizierung und Sitzungszustand, Anbieterendpunkte, Echtzeitübertragung und Abfrageintervalle.

- `AuthorizationCode` (`string`)
- `SessionToken` (`SecureString`)
- `PortfolioName` (`string`)
- `RestEndpoint` (`string`)
- `BridgeHost` (`string`)
- `BridgePort` (`int`)
- `TokenValidationEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_iifl.md)

[Adapterinitialisierung](adapter_initialization_iifl.md)
