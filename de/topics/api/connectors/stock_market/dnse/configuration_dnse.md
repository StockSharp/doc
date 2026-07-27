# Connector-Konfiguration: DNSE

Konfigurieren Sie vor der Verbindung mit DNSE die folgenden Adaptereigenschaften. Die Liste wurde anhand von [DnseMessageAdapter](xref:StockSharp.Dnse.DnseMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `TradingToken` (`SecureString`)
- `Account` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `OtpType` (`DnseOtpTypes`)
- `OneTimePassword` (`SecureString`)
- `RequestEmailOtpOnConnect` (`bool`)
- `DefaultLoanPackageId` (`int`)
- `DefaultBoardId` (`string`)
- `MarketDataPriceMultiplier` (`decimal`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `ApiVersion` (`string`)
- `DateHeaderName` (`string`)
- `RestAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_dnse.md)

[Adapterinitialisierung](adapter_initialization_dnse.md)
