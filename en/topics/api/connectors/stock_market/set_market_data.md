# SET Market Data

The **SET Market Data connector** connects StockSharp to a professional market-data and analytics service. It translates provider-specific data and operations into the unified StockSharp message model, so applications can use the same subscriptions and workflows across different venues.

## Key capabilities

- Typical coverage: equities.
- Instrument discovery and provider reference data.
- Provider-supported market, company, filing, disclosure, and reference data.
- Market data supported by the adapter: Level 1 quotes and order books.
- Real-time subscriptions through the provider's streaming transport.
- This adapter is intended for data access and does not route orders.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to feed charts, market-data storage, analytics, research workflows, and strategy testing with provider data.

Available instruments, data depth, trading permissions, rate limits, and service availability are controlled by SET Market Data and by the connected account or API plan.

## See also

[Connector configuration](set_market_data/configuration_set_market_data.md)

[Graphical configuration](set_market_data/graphical_configuration_set_market_data.md)

[Adapter initialization](set_market_data/adapter_initialization_set_market_data.md)
