# comdirect

The **comdirect connector** connects StockSharp to a broker or electronic venue for financial markets. It translates provider-specific data and operations into the unified StockSharp message model, so applications can use the same subscriptions and workflows across different venues.

## Key capabilities

- Typical coverage: equities.
- Instrument discovery and provider reference data.
- Provider-supported order submission and execution workflows.
- Portfolio, balance, position, and execution-state updates.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector for live strategies, trading terminals, order-management services, and monitoring tools that need direct access to the provider.

Available instruments, data depth, trading permissions, rate limits, and service availability are controlled by comdirect and by the connected account or API plan.

## See also

[Connector configuration](comdirect/configuration_comdirect.md)

[Graphical configuration](comdirect/graphical_configuration_comdirect.md)

[Adapter initialization](comdirect/adapter_initialization_comdirect.md)
