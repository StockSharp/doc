# Connector configuration: StocksTrader

Generate a token in the StocksTrader web terminal and specify the connection parameters.

- `Token` - bearer token issued by the web terminal.
- `AccountId` - account identifier. Optional when exactly one account matches the selected mode.
- `IsDemo` - selects the demo account. The default is `true`.
- `Address` - REST endpoint. The default is `https://api.stockstrader.com/`.
- `PollingInterval` - how often orders, deals, and the account state are requested. The default is 5 seconds, and shorter values are raised to 2 seconds.

Because the provider offers no streaming, `PollingInterval` defines how quickly order and position changes reach the strategy. Shorten it for active trading and lengthen it to stay within the provider request limits.

Protective prices are passed through [StocksTraderOrderCondition](xref:StockSharp.StocksTrader.StocksTraderOrderCondition): the stop-loss and take-profit prices of the order or of the open position.

## See also

[Official StocksTrader API documentation](https://api-doc.stockstrader.com/)
