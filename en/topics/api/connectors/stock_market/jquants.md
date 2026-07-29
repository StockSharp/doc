# J-Quants

**J-Quants** connects StockSharp to J-Quants API V2 for Tokyo Stock Exchange market data.

## Key capabilities

- Source-verified capabilities: lookup of TSE stocks, futures, and options, Level 1 values, tick data, and historical candles. The adapter is market-data only and does not expose transaction operations.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to discover Japanese instruments and download quotes, trades, and candle history for research and analysis.

Data coverage, history depth, request limits, and availability are controlled by the J-Quants subscription plan.

## See also

[Connector configuration](jquants/configuration_jquants.md)

[Graphical configuration](jquants/graphical_configuration_jquants.md)

[Adapter initialization](jquants/adapter_initialization_jquants.md)
