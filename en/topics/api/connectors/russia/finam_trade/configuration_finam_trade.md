# Connector configuration: Finam Trade API

Configure the following properties before connecting to Finam. The list is verified against [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`) — required Finam Trade API secret. The adapter exchanges it for a short-lived session token.
- `AccountId` (`string`) — optional trading account identifier. When empty, the adapter uses the first account available to the token.

## Advanced settings

- `AppId` (`string`) — application identifier sent during session creation. The default is `StockSharp`.
- `PollingInterval` (`TimeSpan`) — interval for account and order snapshot polling. The default is 30 seconds; values below one second are rejected.
- `LookupLimit` (`int`) — maximum number of instruments returned by an unrestricted lookup. The default is `10000`, and the value must be positive.
- `RestAddress` (`string`) — REST API base address. The default is `https://api.finam.ru/`.
- `WebSocketAddress` (`string`) — WebSocket API address. The default is `wss://api.finam.ru/ws`.

Keep the prefilled addresses unless Finam or a compatible gateway assigned different endpoints.

## See also

[Graphical configuration](graphical_configuration_finam_trade.md)

[Adapter initialization](adapter_initialization_finam_trade.md)
