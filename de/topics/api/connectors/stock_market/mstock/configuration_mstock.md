# Connector-Konfiguration: m.Stock

Konfigurieren Sie vor der Verbindung mit m.Stock die folgenden Adaptereigenschaften. Die Liste wurde anhand von [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `ClientCode` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Authentifizierung und Sitzungszustand, Anbieterendpunkte, Echtzeitübertragung und Abfrageintervalle.

- `Password` (`SecureString`)
- `Otp` (`SecureString`)
- `UseTotp` (`bool`)
- `RefreshToken` (`SecureString`)
- `AccessToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_mstock.md)

[Adapterinitialisierung](adapter_initialization_mstock.md)
