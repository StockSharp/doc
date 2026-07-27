# Connector configuration: Choice FinX

Configure the following properties before connecting to Choice FinX. The list is verified against [ChoiceFinXMessageAdapter](xref:StockSharp.ChoiceFinX.ChoiceFinXMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `AuthorizationHeader` (`string`)
- `AuthorizationScheme` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `VendorId` (`string`)
- `VendorKey` (`SecureString`)
- `WebSocketToken` (`SecureString`)
- `DefaultProduct` (`ChoiceFinXProducts`)
- `PortfolioName` (`string`)
- `ModeType` (`string`)
- `Mode` (`int?`)
- `DeviceId` (`string`)
- `PriceDivisor` (`decimal`)
- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_choice_finx.md)

[Adapter initialization](adapter_initialization_choice_finx.md)
