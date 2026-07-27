# Connector-Konfiguration: JPX TDnet

Konfigurieren Sie vor der Verbindung mit JPX TDnet die folgenden Adaptereigenschaften. Die Liste wurde anhand von [JpxTdnetMessageAdapter](xref:StockSharp.JpxTdnet.JpxTdnetMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Address` (`Uri`)
- `ViewerAddress` (`Uri`)
- `IndexMode` (`JpxTdnetIndexModes`)
- `DefaultLookupDays` (`int`)
- `MaxDays` (`int`)
- `SecurityLookupDays` (`int`)
- `RequestInterval` (`TimeSpan`)
- `MaxDocumentSizeMb` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_jpx_tdnet.md)

[Adapterinitialisierung](adapter_initialization_jpx_tdnet.md)
