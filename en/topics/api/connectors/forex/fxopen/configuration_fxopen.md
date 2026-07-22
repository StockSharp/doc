# Connector configuration: FXOpen TickTrader

Create an FXOpen Web API token and specify the connection parameters.

- `WebApiId` - Web API token identifier.
- `Key` - Web API key.
- `Secret` - Web API secret.
- `OneTimePassword` - optional one-time password when two-factor authentication is required.
- `IsDemo` - selects the demo environment. The default is `false`.
- `Address` - REST endpoint. The live default is `https://ttlivewebapi.fxopen.net`.
- `FeedAddress` - Feed WebSocket endpoint. The live default is `wss://marginalttlivewebapi.fxopen.net/feed`.
- `TradeAddress` - Trade WebSocket endpoint. The live default is `wss://marginalttlivewebapi.fxopen.net/trade`.

Enabling `IsDemo` selects the official marginal TickTrader demo endpoints unless an endpoint was customized. The ID, key, and secret are required for WebSocket subscriptions and all private operations.

## See also

[Official FXOpen API documentation](https://ticktrader.fxopen.com/api)

[TickTrader Web REST API](https://ttlivewebapi.fxopen.net/api/doc/index?apiaddress=ttlivewebapi.fxopen.net&apiport=443)
