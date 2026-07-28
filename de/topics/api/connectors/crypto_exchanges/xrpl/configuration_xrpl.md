# Connector-Konfiguration: XRPL DEX

Konfigurieren Sie vor der Verbindung mit XRPL DEX die folgenden Adaptereigenschaften. Die Liste wurde anhand von [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `RpcEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `Account` (`string`)
- `Seed` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Marktauswahl, Auftragsbuchtiefe, Historie, Gebühren, Abfragen und Transaktionsschutz.

- `Markets` (`string`)
- `DomainId` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLedgerLimit` (`int`)
- `FeeMultiplier` (`decimal`)
- `LastLedgerOffset` (`int`)
- `MarketOrderProtection` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_xrpl.md)

[Adapterinitialisierung](adapter_initialization_xrpl.md)
