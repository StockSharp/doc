# MarketData.app

**MarketData.app** connects StockSharp to MarketData.app for US stocks, funds, indices, and options.

## Key capabilities

- Source-verified capabilities: instrument and option-contract lookup, Level 1 market data, and historical candles with extended-hours and split-adjustment options. The adapter does not expose trading operations.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to find US instruments, obtain quotes, and download stock or option candle history.

Data coverage, delay, history depth, contract limits, and availability depend on the MarketData.app subscription plan.

## See also

[Connector configuration](marketdataapp/configuration_marketdataapp.md)

[Graphical configuration](marketdataapp/graphical_configuration_marketdataapp.md)

[Adapter initialization](marketdataapp/adapter_initialization_marketdataapp.md)
