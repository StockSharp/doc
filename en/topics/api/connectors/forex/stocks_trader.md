# StocksTrader

**StocksTrader** connects StockSharp to the official StocksTrader REST API.

The connector provides demo and real account discovery, account-state polling, instrument lookup, and the latest bid, ask, and last-price snapshots. Trading covers market, limit, and stop orders, pending-order modification and cancellation, stop-loss and take-profit modification for open positions, position closing, and order and deal history.

StocksTrader has no streaming market-data API, so a Level 1 request returns the latest available snapshot and completes immediately, while orders, deals, and the account state are polled.

Create a bearer token in the StocksTrader web terminal before connecting.

## See also

[Connector configuration](stocks_trader/configuration_stocks_trader.md)

[Graphical configuration](stocks_trader/graphical_configuration_stocks_trader.md)

[Adapter initialization](stocks_trader/adapter_initialization_stocks_trader.md)

[Official StocksTrader API documentation](https://api-doc.stockstrader.com/)
