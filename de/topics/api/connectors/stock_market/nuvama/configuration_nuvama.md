# Connector-Konfiguration: Nuvama

Konfigurieren Sie vor der Verbindung mit Nuvama die folgenden Adaptereigenschaften. Die Liste wurde anhand von [NuvamaMessageAdapter](xref:StockSharp.Nuvama.NuvamaMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RequestId` (`SecureString`)
- `AppIdKey` (`SecureString`)
- `PublicIpAddress` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `VendorToken` (`SecureString`)
- `Token` (`SecureString`)
- `AccountId` (`string`)
- `UserId` (`string`)
- `AccountType` (`string`)
- `EmployeeOrDependent` (`string`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`NuvamaProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`Uri`)
- `IpAddressService` (`Uri`)
- `StreamHost` (`string`)
- `StreamPort` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_nuvama.md)

[Adapterinitialisierung](adapter_initialization_nuvama.md)
