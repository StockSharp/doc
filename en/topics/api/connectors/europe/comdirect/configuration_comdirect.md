# Connector configuration: comdirect

Configure the following properties before connecting to comdirect. The list is verified against [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Login` (`string`)
- `Password` (`SecureString`)
- `TanType` (`ComdirectTanTypes`)
- `PollingInterval` (`TimeSpan`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `DefaultCurrency` (`string`)
- `Address` (`Uri`)

## See also

[Graphical configuration](graphical_configuration_comdirect.md)

[Adapter initialization](adapter_initialization_comdirect.md)
