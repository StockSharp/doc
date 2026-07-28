# Pendle

**Pendle** connects StockSharp to Pendle yield markets on supported EVM networks. The adapter exposes supported market data and transaction operations through the standard StockSharp message model.

## Key capabilities

- Adapter capabilities verified in the source code: real-time data updates, Level 1 quotes, candle data, historical data requests, portfolio and order operations.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to monitor Pendle yield markets, analyze quotes and candles, request history, and run the transaction workflows implemented by the protocol adapter.

Available networks, markets, transaction permissions, request limits, and service availability are controlled by Pendle and by the connected wallet.

## See also

[Connector configuration](pendle/configuration_pendle.md)

[Graphical configuration](pendle/graphical_configuration_pendle.md)

[Adapter initialization](pendle/adapter_initialization_pendle.md)
