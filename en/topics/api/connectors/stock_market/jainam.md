# Jainam

The **Jainam connector** connects StockSharp to a broker or electronic venue for financial markets. It translates provider-specific data and operations into the unified StockSharp message model, so applications can use the same subscriptions and workflows across different venues.

## Key capabilities

- Typical coverage: equities, futures, options, FX and commodities.
- Instrument discovery and provider reference data.
- Market data supported by the adapter: Level 1 quotes, tick trades and order books.
- Provider-supported order submission and execution workflows.
- Portfolio, balance, position, and execution-state updates.
- Real-time subscriptions through the provider's streaming transport.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector for live strategies, trading terminals, order-management services, and monitoring tools that need direct access to the provider.

Available instruments, data depth, trading permissions, rate limits, and service availability are controlled by Jainam and by the connected account or API plan.

## See also

[Connector configuration](jainam/configuration_jainam.md)

[Graphical configuration](jainam/graphical_configuration_jainam.md)

[Adapter initialization](jainam/adapter_initialization_jainam.md)
