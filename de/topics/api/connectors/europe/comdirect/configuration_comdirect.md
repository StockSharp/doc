# Connector-Konfiguration: comdirect

Konfigurieren Sie vor der Verbindung mit comdirect die folgenden Adaptereigenschaften. Die Liste wurde anhand von [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Login` (`string`)
- `Password` (`SecureString`)
- `TanType` (`ComdirectTanTypes`)
- `PollingInterval` (`TimeSpan`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `DefaultCurrency` (`string`)
- `Address` (`Uri`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_comdirect.md)

[Adapterinitialisierung](adapter_initialization_comdirect.md)
