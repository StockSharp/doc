# XRPL DEX

**XRPL DEX** connects StockSharp to the decentralized exchange built into the XRP Ledger. The adapter exposes supported market data and transaction operations through the standard StockSharp message model.

## Key capabilities

- Adapter capabilities verified in the source code: real-time data updates, Level 1 quotes, trade ticks, order books, candle data, historical data requests, portfolio and order operations.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to monitor XRPL markets, analyze quotes, trades, order books, and candles, request history, and submit supported offers.

Available markets, data depth, transaction permissions, request limits, and service availability are controlled by the XRP Ledger endpoints and by the connected account.

## See also

[Connector configuration](xrpl/configuration_xrpl.md)

[Graphical configuration](xrpl/graphical_configuration_xrpl.md)

[Adapter initialization](xrpl/adapter_initialization_xrpl.md)
