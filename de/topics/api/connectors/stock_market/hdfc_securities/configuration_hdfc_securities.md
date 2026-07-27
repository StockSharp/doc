# Connector-Konfiguration: HDFC Securities

Konfigurieren Sie vor der Verbindung mit HDFC Securities die folgenden Adaptereigenschaften. Die Liste wurde anhand von [HdfcMessageAdapter](xref:StockSharp.HdfcSecurities.HdfcMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RequestToken` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Token` (`SecureString`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`HdfcProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_hdfc_securities.md)

[Adapterinitialisierung](adapter_initialization_hdfc_securities.md)
