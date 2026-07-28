# Connector-Konfiguration: Birdeye

Konfigurieren Sie vor der Verbindung mit Birdeye die folgenden Adaptereigenschaften. Die Liste wurde anhand von [BirdeyeMessageAdapter](xref:StockSharp.Birdeye.BirdeyeMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `WebSocketOrigin` (`string`)
- `Chain` (`string`)
- `TokenAddress` (`string`)
- `StreamingEnabled` (`bool`)
- `PriceInUsd` (`bool`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `MinimumLiquidity` (`decimal`)
- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_birdeye.md)

[Adapterinitialisierung](adapter_initialization_birdeye.md)
