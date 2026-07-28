# Connector configuration: CoinSwitch PRO

Configure the following properties before connecting to CoinSwitch PRO. The list is verified against [CoinSwitchMessageAdapter](xref:StockSharp.CoinSwitch.CoinSwitchMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ProductType` (`CoinSwitchProductTypes`)
- `SpotExchange` (`string`)
- `RestEndpoint` (`string`)
- `HftEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_coinswitch.md)

[Adapter initialization](adapter_initialization_coinswitch.md)
