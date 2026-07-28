# Connector-Konfiguration: Dexalot

Konfigurieren Sie vor der Verbindung mit Dexalot die folgenden Adaptereigenschaften. Die Liste wurde anhand von [DexalotMessageAdapter](xref:StockSharp.Dexalot.DexalotMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `RpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Vertragsadressen, Paarfilter, Auftragsbuchtiefe, Abfragen und Transaktionsverhalten.

- `TradePairsAddress` (`string`)
- `PortfolioAddress` (`string`)
- `Pairs` (`string`)
- `OrderBookDepth` (`int`)
- `PrivatePollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `SelfTradePrevention` (`DexalotSelfTradePrevention`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_dexalot.md)

[Adapterinitialisierung](adapter_initialization_dexalot.md)
