# Connector-Konfiguration: Nubra

Konfigurieren Sie vor der Verbindung mit Nubra die folgenden Adaptereigenschaften. Die Liste wurde anhand von [NubraMessageAdapter](xref:StockSharp.Nubra.NubraMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `DeviceId` (`string`)
- `IsDemo` (`bool`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Phone` (`string`)
- `Mpin` (`SecureString`)
- `TotpSecret` (`SecureString`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`NubraProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `UatRestAddress` (`Uri`)
- `MarketDataAddress` (`Uri`)
- `UatMarketDataAddress` (`Uri`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_nubra.md)

[Adapterinitialisierung](adapter_initialization_nubra.md)
