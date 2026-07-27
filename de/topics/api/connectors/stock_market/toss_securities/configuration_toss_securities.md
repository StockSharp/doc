# Connector-Konfiguration: Toss Securities

Konfigurieren Sie vor der Verbindung mit Toss Securities die folgenden Adaptereigenschaften. Die Liste wurde anhand von [TossSecuritiesMessageAdapter](xref:StockSharp.TossSecurities.TossSecuritiesMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AccountSequence` (`long`)
- `PollingInterval` (`TimeSpan`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `PortfolioName` (`string`)
- `AccountPollingInterval` (`TimeSpan`)
- `AdjustedCandles` (`bool`)
- `RestAddress` (`Uri`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_toss_securities.md)

[Adapterinitialisierung](adapter_initialization_toss_securities.md)
