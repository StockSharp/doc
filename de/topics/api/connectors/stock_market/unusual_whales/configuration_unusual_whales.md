# Connector-Konfiguration: Unusual Whales

Konfigurieren Sie vor der Verbindung mit Unusual Whales die folgenden Adaptereigenschaften. Die Liste wurde anhand von [UnusualWhalesMessageAdapter](xref:StockSharp.UnusualWhales.UnusualWhalesMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `CandleLimit` (`int`)
- `NewsLimit` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `UnusualFlowOnly` (`bool`)
- `OtmMarketTide` (`bool`)
- `FiveMinuteMarketTide` (`bool`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_unusual_whales.md)

[Adapterinitialisierung](adapter_initialization_unusual_whales.md)
