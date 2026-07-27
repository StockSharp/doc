# Connector-Konfiguration: Marketaux

Konfigurieren Sie vor der Verbindung mit Marketaux die folgenden Adaptereigenschaften. Die Liste wurde anhand von [MarketauxMessageAdapter](xref:StockSharp.Marketaux.MarketauxMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Languages` (`string`)
- `EntityTypes` (`string`)
- `Countries` (`string`)
- `MustHaveEntities` (`bool`)
- `GroupSimilar` (`bool`)
- `NewsPageSize` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `SentimentInterval` (`MarketauxIntervals`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_marketaux.md)

[Adapterinitialisierung](adapter_initialization_marketaux.md)
