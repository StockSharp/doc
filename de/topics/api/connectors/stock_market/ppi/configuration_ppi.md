# Connector-Konfiguration: PPI

Konfigurieren Sie vor der Verbindung mit PPI die folgenden Adaptereigenschaften. Die Liste wurde anhand von [PpiMessageAdapter](xref:StockSharp.Ppi.PpiMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AuthorizedClient` (`string`)
- `ClientKey` (`SecureString`)
- `IsDemo` (`bool`)
- `Account` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Token` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `DefaultMarket` (`string`)
- `DefaultInstrumentType` (`string`)
- `DefaultSettlement` (`string`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)
- `RealtimeAddress` (`Uri`)
- `SandboxRealtimeAddress` (`Uri`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_ppi.md)

[Adapterinitialisierung](adapter_initialization_ppi.md)
