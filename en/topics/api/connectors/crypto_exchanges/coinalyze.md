# Coinalyze

**Coinalyze** connects StockSharp to the Coinalyze cryptocurrency market-data service. The adapter exposes provider data through the standard StockSharp message model.

## Key capabilities

- Adapter capabilities verified in the source code: candle data, historical data requests, futures markets.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.
- This adapter is intended for data access and does not route orders.

## Typical use

Use this connector to feed charts, market-data storage, analytics, research, and strategy testing with provider data.

Available instruments, data depth, transaction permissions, request limits, and service availability are controlled by Coinalyze and by the connected account or API plan.

## See also

[Connector configuration](coinalyze/configuration_coinalyze.md)

[Graphical configuration](coinalyze/graphical_configuration_coinalyze.md)

[Adapter initialization](coinalyze/adapter_initialization_coinalyze.md)
