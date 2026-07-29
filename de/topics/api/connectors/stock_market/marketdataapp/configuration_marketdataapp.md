# Connector-Konfiguration: MarketData.app

Konfigurieren Sie vor der Verbindung mit MarketData.app die folgenden Adaptereigenschaften. Die Liste wurde anhand von [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `RestEndpoint` (`Uri`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Anbieterendpunkte, Anfrageabstände, Filter, Datenoptionen und Ergebnislimits.

- `ExtendedHours` (`bool`)
- `AdjustSplits` (`bool`)
- `MaximumOptionContracts` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_marketdataapp.md)

[Adapterinitialisierung](adapter_initialization_marketdataapp.md)
