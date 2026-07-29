# Connector-Konfiguration: TraderMade

Konfigurieren Sie vor der Verbindung mit TraderMade die folgenden Adaptereigenschaften. Die Liste wurde anhand von [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `RestKey` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern REST- und WebSocket-Zugriff, Endpunkte, Marktdatenoptionen, Symbolfilter und Ergebnislimits.

- `StreamingKey` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `EnableLadder` (`bool`)
- `Weekend` (`bool`)
- `QuoteCurrencies` (`string`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_tradermade.md)

[Adapterinitialisierung](adapter_initialization_tradermade.md)
