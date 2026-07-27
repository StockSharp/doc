# Connector-Konfiguration: MasterLink

Konfigurieren Sie vor der Verbindung mit MasterLink die folgenden Adaptereigenschaften. Die Liste wurde anhand von [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Login` (`string`)
- `Password` (`SecureString`)
- `CertificatePath` (`string`)
- `CertificatePassword` (`SecureString`)
- `NodePath` (`string`)
- `GatewayDirectory` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Account` (`string`)
- `RegisterApiAuth` (`bool`)
- `MarketDataMode` (`MasterLinkMarketDataModes`)
- `AdjustedCandles` (`bool`)
- `AccountPollingInterval` (`TimeSpan`)
- `MaxLookupResults` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_masterlink.md)

[Adapterinitialisierung](adapter_initialization_masterlink.md)
