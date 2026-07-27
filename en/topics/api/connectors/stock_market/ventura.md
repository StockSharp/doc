# Ventura

The **Ventura connector** connects StockSharp to a broker or electronic venue for financial markets. It translates provider-specific data and operations into the unified StockSharp message model, so applications can use the same subscriptions and workflows across different venues.

## Key capabilities

- Typical coverage: equities, futures and options.
- Instrument discovery and provider reference data.
- Market data supported by the adapter: Level 1 quotes, tick trades and order books.
- Provider-supported order submission and execution workflows.
- Portfolio, balance, position, and execution-state updates.
- Real-time subscriptions through the provider's streaming transport.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector for live strategies, trading terminals, order-management services, and monitoring tools that need direct access to the provider.

Available instruments, data depth, trading permissions, rate limits, and service availability are controlled by Ventura and by the connected account or API plan.

## See also

[Connector configuration](ventura/configuration_ventura.md)

[Graphical configuration](ventura/graphical_configuration_ventura.md)

[Adapter initialization](ventura/adapter_initialization_ventura.md)
