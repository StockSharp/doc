# Connector configuration: Deriv

Create a Deriv token and specify the connection parameters.

- `Token` - personal access or OAuth token. Required for private operations.
- `AppId` - application identifier sent with authenticated REST requests.
- `AccountId` - options account identifier. Optional when exactly one active account matches the selected mode.
- `IsDemo` - selects the demo account. The default is `true`.
- `RestAddress` - REST endpoint. The default is `https://api.derivws.com`.
- `PublicWebSocketAddress` - public options WebSocket endpoint. The default is `wss://api.derivws.com/trading/v1/options/ws/public`.

Public market-data sessions run without a token, while contracts, balances, and transactions require the token and the application ID. Subscriptions are restored automatically over a fresh one-time WebSocket address.

Contract parameters are passed through [DerivOrderCondition](xref:StockSharp.Deriv.DerivOrderCondition): contract type, whether the amount is a stake or a payout, contract currency, duration, barriers, and the protective stop-loss and take-profit prices.

## See also

[Official Deriv API documentation](https://developers.deriv.com/docs/)
