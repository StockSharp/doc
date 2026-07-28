# Connector-Konfiguration: Coincall

Konfigurieren Sie vor der Verbindung mit Coincall die folgenden Adaptereigenschaften. Die Liste wurde anhand von [CoincallMessageAdapter](xref:StockSharp.Coincall.CoincallMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ProductType` (`CoincallProductTypes`)
- `RestEndpoint` (`string`)
- `OptionsWebSocketEndpoint` (`string`)
- `FuturesWebSocketEndpoint` (`string`)
- `RequestValidityWindow` (`TimeSpan`)
- `PrivatePollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_coincall.md)

[Adapterinitialisierung](adapter_initialization_coincall.md)
