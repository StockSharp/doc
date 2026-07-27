# Connector-Konfiguration: EDINET

Konfigurieren Sie vor der Verbindung mit EDINET die folgenden Adaptereigenschaften. Die Liste wurde anhand von [EdinetMessageAdapter](xref:StockSharp.Edinet.EdinetMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Address` (`Uri`)
- `CodeListAddress` (`Uri`)
- `ViewerAddress` (`Uri`)
- `DisclosureType` (`EdinetDisclosureTypes`)
- `ListedOnly` (`bool`)
- `IncludeWithdrawn` (`bool`)
- `IncludeUnavailable` (`bool`)
- `DefaultLookupDays` (`int`)
- `MaxDays` (`int`)
- `RequestInterval` (`TimeSpan`)
- `MaxDocumentSizeMb` (`int`)
- `CodeListCacheTimeout` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_edinet.md)

[Adapterinitialisierung](adapter_initialization_edinet.md)
