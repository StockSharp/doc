# Connector-Konfiguration: Velodrome

Konfigurieren Sie vor der Verbindung mit Velodrome die folgenden Adaptereigenschaften. Die Liste wurde anhand von [VelodromeMessageAdapter](xref:StockSharp.Velodrome.VelodromeMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `RpcEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Pools` (`string`)
- `HistoryBlockRange` (`int`)
- `HistoryBlockCount` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_velodrome.md)

[Adapterinitialisierung](adapter_initialization_velodrome.md)
