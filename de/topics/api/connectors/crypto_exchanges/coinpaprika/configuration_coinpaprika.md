# Connector-Konfiguration: CoinPaprika

Konfigurieren Sie vor der Verbindung mit CoinPaprika die folgenden Adaptereigenschaften. Die Liste wurde anhand von [CoinPaprikaMessageAdapter](xref:StockSharp.CoinPaprika.CoinPaprikaMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `QuoteCurrency` (`string`)
- `ExchangeId` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_coinpaprika.md)

[Adapterinitialisierung](adapter_initialization_coinpaprika.md)
