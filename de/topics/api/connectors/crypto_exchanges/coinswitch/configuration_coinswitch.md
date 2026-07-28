# Connector-Konfiguration: CoinSwitch PRO

Konfigurieren Sie vor der Verbindung mit CoinSwitch PRO die folgenden Adaptereigenschaften. Die Liste wurde anhand von [CoinSwitchMessageAdapter](xref:StockSharp.CoinSwitch.CoinSwitchMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ProductType` (`CoinSwitchProductTypes`)
- `SpotExchange` (`string`)
- `RestEndpoint` (`string`)
- `HftEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_coinswitch.md)

[Adapterinitialisierung](adapter_initialization_coinswitch.md)
