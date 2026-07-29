# Connector configuration: m.Stock

Configure the following properties before connecting to m.Stock. The list is verified against [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `ClientCode` (`string`)

## Advanced settings

These properties control authentication and session state, provider endpoints, streaming, and polling.

- `Password` (`SecureString`)
- `Otp` (`SecureString`)
- `UseTotp` (`bool`)
- `RefreshToken` (`SecureString`)
- `AccessToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_mstock.md)

[Adapter initialization](adapter_initialization_mstock.md)
