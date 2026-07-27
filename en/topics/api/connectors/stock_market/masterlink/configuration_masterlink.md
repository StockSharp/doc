# Connector configuration: MasterLink

Configure the following properties before connecting to MasterLink. The list is verified against [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Login` (`string`)
- `Password` (`SecureString`)
- `CertificatePath` (`string`)
- `CertificatePassword` (`SecureString`)
- `NodePath` (`string`)
- `GatewayDirectory` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Account` (`string`)
- `RegisterApiAuth` (`bool`)
- `MarketDataMode` (`MasterLinkMarketDataModes`)
- `AdjustedCandles` (`bool`)
- `AccountPollingInterval` (`TimeSpan`)
- `MaxLookupResults` (`int`)

## See also

[Graphical configuration](graphical_configuration_masterlink.md)

[Adapter initialization](adapter_initialization_masterlink.md)
