# Connector configuration: Tradernet

Configure the following properties before connecting to Tradernet. The list is verified against [TradernetMessageAdapter](xref:StockSharp.Tradernet.TradernetMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `PollingInterval` (`TimeSpan`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `MaxMarketDepth` (`int`)
- `SecuritiesPageSize` (`int`)

## See also

[Graphical configuration](graphical_configuration_tradernet.md)

[Adapter initialization](adapter_initialization_tradernet.md)
