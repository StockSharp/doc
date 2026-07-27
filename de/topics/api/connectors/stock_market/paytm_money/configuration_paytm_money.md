# Connector-Konfiguration: Paytm Money

Konfigurieren Sie vor der Verbindung mit Paytm Money die folgenden Adaptereigenschaften. Die Liste wurde anhand von [PaytmMoneyMessageAdapter](xref:StockSharp.PaytmMoney.PaytmMoneyMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `ReadAccessToken` (`SecureString`)
- `PublicAccessToken` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `RequestToken` (`SecureString`)
- `DefaultProduct` (`PaytmMoneyProducts`)
- `PortfolioName` (`string`)
- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `SecurityMasterFile` (`string`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_paytm_money.md)

[Adapterinitialisierung](adapter_initialization_paytm_money.md)
