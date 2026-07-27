# Connector configuration: Nuvama

Configure the following properties before connecting to Nuvama. The list is verified against [NuvamaMessageAdapter](xref:StockSharp.Nuvama.NuvamaMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RequestId` (`SecureString`)
- `AppIdKey` (`SecureString`)
- `PublicIpAddress` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

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

## See also

[Graphical configuration](graphical_configuration_nuvama.md)

[Adapter initialization](adapter_initialization_nuvama.md)
