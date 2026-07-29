# Finam Trade API

The **Finam Trade API connector** connects StockSharp applications to Finam brokerage accounts and market data. It converts Finam instruments, quotes, orders, trades, and portfolio state into the standard StockSharp message model.

## Key capabilities

- Instrument discovery for stocks, bonds, currencies, funds, futures, and options available through Finam.
- Level 1 quotes, order books, public trades, and time-frame candles.
- Historical candle requests and real-time market-data subscriptions.
- Market, limit, stop, and stop-limit order submission and order cancellation.
- Order-state, own-trade, cash, and position updates.
- Automatic exchange of the API secret for a short-lived session token.
- Configurable REST and WebSocket addresses for compatible gateways and test environments.

## Typical use

Use the connector in trading robots, terminals, portfolio monitors, and order-management services that need one StockSharp interface for Finam market data and trading.

A Finam Trade API secret is required. Select an account explicitly or leave the account identifier empty to use the first account available to the token. Instruments use Finam's `ticker@MIC` format. Available markets, history depth, real-time data, trading permissions, and request limits depend on the connected account and Finam service terms.

## See also

[Connector configuration](finam_trade/configuration_finam_trade.md)

[Graphical configuration](finam_trade/graphical_configuration_finam_trade.md)

[Adapter initialization](finam_trade/adapter_initialization_finam_trade.md)
