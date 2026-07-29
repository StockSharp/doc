# Connector configuration: TraderMade

Configure the following properties before connecting to TraderMade. The list is verified against [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `RestKey` (`SecureString`)

## Advanced settings

These properties control REST and WebSocket access, endpoints, market-data options, symbol filters, and result limits.

- `StreamingKey` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `EnableLadder` (`bool`)
- `Weekend` (`bool`)
- `QuoteCurrencies` (`string`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## See also

[Graphical configuration](graphical_configuration_tradermade.md)

[Adapter initialization](adapter_initialization_tradermade.md)
