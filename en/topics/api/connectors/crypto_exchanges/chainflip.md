# Chainflip

**Chainflip** connects StockSharp to the Chainflip cross-chain liquidity network. The adapter exposes supported market data and transaction operations through the standard StockSharp message model.

## Key capabilities

- Adapter capabilities verified in the source code: real-time data updates, Level 1 quotes, trade ticks, order books, portfolio and order operations.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to monitor Chainflip liquidity pools, analyze executable quotes and order books, and run the cross-chain swap workflows implemented by the adapter.

Available assets, destination chains, transaction permissions, request limits, and service availability are controlled by Chainflip and by the connected wallet.

## See also

[Connector configuration](chainflip/configuration_chainflip.md)

[Graphical configuration](chainflip/graphical_configuration_chainflip.md)

[Adapter initialization](chainflip/adapter_initialization_chainflip.md)
