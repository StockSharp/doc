# Connector configuration: Nubra

Configure the following properties before connecting to Nubra. The list is verified against [NubraMessageAdapter](xref:StockSharp.Nubra.NubraMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `DeviceId` (`string`)
- `IsDemo` (`bool`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

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

## See also

[Graphical configuration](graphical_configuration_nubra.md)

[Adapter initialization](adapter_initialization_nubra.md)
