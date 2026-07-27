# Connector-Konfiguration: Directa

Konfigurieren Sie vor der Verbindung mit Directa die folgenden Adaptereigenschaften. Die Liste wurde anhand von [DirectaMessageAdapter](xref:StockSharp.Directa.DirectaMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Address` (`EndPoint`)
- `DataAddress` (`EndPoint`)
- `HistoryAddress` (`EndPoint`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `RequestTimeout` (`TimeSpan`)
- `AutoConfirmOrders` (`bool`)
- `MaxMarketDepth` (`int`)
- `TimeZoneId` (`string`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_directa.md)

[Adapterinitialisierung](adapter_initialization_directa.md)
