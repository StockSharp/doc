# CoinGlass

**CoinGlass** connects StockSharp to the CoinGlass cryptocurrency market-data service. The adapter exposes provider data through the standard StockSharp message model.

## Key capabilities

- Adapter capabilities verified in the source code: real-time data updates, Level 1 quotes, candle data, historical data requests, futures markets, options markets.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.
- This adapter is intended for data access and does not route orders.

## Typical use

Use this connector to feed charts, market-data storage, analytics, research, and strategy testing with provider data.

Available instruments, data depth, transaction permissions, request limits, and service availability are controlled by CoinGlass and by the connected account or API plan.

## See also

[Connector configuration](coinglass/configuration_coinglass.md)

[Graphical configuration](coinglass/graphical_configuration_coinglass.md)

[Adapter initialization](coinglass/adapter_initialization_coinglass.md)
