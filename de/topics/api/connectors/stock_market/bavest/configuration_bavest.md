# Connector-Konfiguration: Bavest

Konfigurieren Sie vor der Verbindung mit Bavest die folgenden Adaptereigenschaften. Die Liste wurde anhand von [BavestMessageAdapter](xref:StockSharp.Bavest.BavestMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Currency` (`string`)
- `Exchange` (`string`)
- `ExchangeCode` (`string`)
- `FinancialFrequency` (`BavestFinancialFrequencies`)
- `TraceEtfMetrics` (`bool`)
- `ScreenerQuery` (`string`)
- `PageSize` (`int`)
- `MaxPages` (`int`)
- `NewsLimit` (`int`)
- `DatasetLimit` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_bavest.md)

[Adapterinitialisierung](adapter_initialization_bavest.md)
