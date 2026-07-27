# Connector-Konfiguration: Definedge

Konfigurieren Sie vor der Verbindung mit Definedge die folgenden Adaptereigenschaften. Die Liste wurde anhand von [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `WebSocketToken` (`SecureString`)
- `UserId` (`string`)
- `AccountId` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `OneTimePassword` (`SecureString`)
- `DefaultProduct` (`DefinedgeProducts`)
- `AlgoId` (`string`)
- `Address` (`Uri`)
- `LoginAddress` (`Uri`)
- `HistoryAddress` (`Uri`)
- `InstrumentMasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_definedge.md)

[Adapterinitialisierung](adapter_initialization_definedge.md)
