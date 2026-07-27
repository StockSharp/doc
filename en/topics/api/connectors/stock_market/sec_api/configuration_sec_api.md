# Connector configuration: SEC API

Configure the following properties before connecting to SEC API. The list is verified against [SecApiMessageAdapter](xref:StockSharp.SecApi.SecApiMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `ActiveOnly` (`bool`)
- `DefaultExchange` (`string`)
- `FormTypes` (`string`)
- `ResultLimit` (`int`)

## See also

[Graphical configuration](graphical_configuration_sec_api.md)

[Adapter initialization](adapter_initialization_sec_api.md)
