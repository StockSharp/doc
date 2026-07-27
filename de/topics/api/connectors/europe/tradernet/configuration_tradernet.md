# Connector-Konfiguration: Tradernet

Konfigurieren Sie vor der Verbindung mit Tradernet die folgenden Adaptereigenschaften. Die Liste wurde anhand von [TradernetMessageAdapter](xref:StockSharp.Tradernet.TradernetMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `PollingInterval` (`TimeSpan`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `MaxMarketDepth` (`int`)
- `SecuritiesPageSize` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_tradernet.md)

[Adapterinitialisierung](adapter_initialization_tradernet.md)
