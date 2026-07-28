# Connector-Konfiguration: Settrade

Konfigurieren Sie vor der Verbindung mit Settrade die folgenden Adaptereigenschaften. Die Liste wurde anhand von [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AppCode` (`string`)
- `BrokerId` (`string`)
- `Account` (`string`)
- `Pin` (`SecureString`)
- `AccountType` (`SettradeAccountTypes`)
- `IsDemo` (`bool`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Anmeldeparameter, Produktions- und Testendpunkte sowie die Abfrage des privaten Zustands.

- `LoginParameters` (`string`)
- `RestEndpoint` (`string`)
- `DemoRestEndpoint` (`string`)
- `MarketDataEndpoint` (`string`)
- `DemoMarketDataEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_settrade.md)

[Adapterinitialisierung](adapter_initialization_settrade.md)
