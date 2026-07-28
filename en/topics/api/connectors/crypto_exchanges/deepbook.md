# DeepBook

**DeepBook** connects StockSharp to the DeepBook decentralized protocol. The adapter exposes supported market data and transaction operations through the standard StockSharp message model.

## Key capabilities

- Adapter capabilities verified in the source code: real-time data updates, Level 1 quotes, Level 2 order books, trade ticks, portfolio and order operations.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector for decentralized-market monitoring, quote analysis, and the transaction workflows implemented by the protocol adapter.

Available instruments, data depth, transaction permissions, request limits, and service availability are controlled by DeepBook and by the connected account or API plan.

## See also

[Connector configuration](deepbook/configuration_deepbook.md)

[Graphical configuration](deepbook/graphical_configuration_deepbook.md)

[Adapter initialization](deepbook/adapter_initialization_deepbook.md)
