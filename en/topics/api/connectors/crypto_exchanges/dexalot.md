# Dexalot

**Dexalot** connects StockSharp to the Dexalot on-chain central limit order book. The adapter exposes supported market data and transaction operations through the standard StockSharp message model.

## Key capabilities

- Adapter capabilities verified in the source code: real-time data updates, Level 1 quotes, trade ticks, order books, candle data, historical data requests, portfolio and order operations.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to monitor Dexalot markets, analyze quotes, trades, order books, and candles, request history, and route supported orders.

Available pairs, data depth, transaction permissions, request limits, and service availability are controlled by Dexalot and by the connected wallet.

## See also

[Connector configuration](dexalot/configuration_dexalot.md)

[Graphical configuration](dexalot/graphical_configuration_dexalot.md)

[Adapter initialization](dexalot/adapter_initialization_dexalot.md)
