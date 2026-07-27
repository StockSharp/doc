# Connector-Konfiguration: StockData.org

Konfigurieren Sie vor der Verbindung mit StockData.org die folgenden Adaptereigenschaften. Die Liste wurde anhand von [StockDataOrgMessageAdapter](xref:StockSharp.StockDataOrg.StockDataOrgMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `ExtendedHours` (`bool`)
- `AdjustedIntraday` (`bool`)
- `NewsLanguage` (`string`)
- `NewsPageSize` (`int`)
- `MaxRequests` (`int`)
- `QuoteTimeZoneId` (`string`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_stockdata_org.md)

[Adapterinitialisierung](adapter_initialization_stockdata_org.md)
