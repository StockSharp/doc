# Settrade

**Settrade** connects StockSharp to Settrade Open API v2 for Stock Exchange of Thailand equities and TFEX derivatives. The adapter exposes supported market data and transaction operations through the standard StockSharp message model.

## Key capabilities

- Adapter capabilities verified in the source code: real-time data updates, Level 1 quotes, order books, candle data, historical data requests, portfolio and order operations for equity and derivatives accounts.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to monitor SET and TFEX instruments, analyze quotes, order books, and candles, request history, and run supported order workflows.

Available instruments, data depth, transaction permissions, request limits, and service availability are controlled by Settrade, the broker, and the connected account.

## See also

[Connector configuration](settrade/configuration_settrade.md)

[Graphical configuration](settrade/graphical_configuration_settrade.md)

[Adapter initialization](settrade/adapter_initialization_settrade.md)
