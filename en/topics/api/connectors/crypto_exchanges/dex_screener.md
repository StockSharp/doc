# DEX Screener

**DEX Screener** connects StockSharp to the DEX Screener cryptocurrency market-data service. The adapter exposes provider data through the standard StockSharp message model.

## Key capabilities

- Adapter capabilities verified in the source code: real-time data updates, Level 1 quotes.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.
- This adapter is intended for data access and does not route orders.

## Typical use

Use this connector to feed charts, market-data storage, analytics, research, and strategy testing with provider data.

Available instruments, data depth, transaction permissions, request limits, and service availability are controlled by DEX Screener and by the connected account or API plan.

## See also

[Connector configuration](dex_screener/configuration_dex_screener.md)

[Graphical configuration](dex_screener/graphical_configuration_dex_screener.md)

[Adapter initialization](dex_screener/adapter_initialization_dex_screener.md)
