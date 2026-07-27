# Connector-Konfiguration: Tradejini

Konfigurieren Sie vor der Verbindung mit Tradejini die folgenden Adaptereigenschaften. Die Liste wurde anhand von [TradejiniMessageAdapter](xref:StockSharp.Tradejini.TradejiniMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `ApiKey` (`SecureString`)
- `Password` (`SecureString`)
- `TwoFactorCode` (`SecureString`)
- `TwoFactorType` (`TradejiniTwoFactorTypes`)
- `Token` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `PortfolioName` (`string`)
- `DefaultProduct` (`TradejiniProducts`)
- `Address` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_tradejini.md)

[Adapterinitialisierung](adapter_initialization_tradejini.md)
