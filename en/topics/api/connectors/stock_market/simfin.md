# SimFin

**SimFin** connects StockSharp to SimFin Web API v3 for company, price, and fundamental data.

## Key capabilities

- Source-verified capabilities: company lookup, daily Level 1 price values, daily candles, and structured fundamentals through `SimFinDataTypes.Fundamentals`. The adapter is historical and does not expose trading operations.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to download daily prices, financial statements, derived values, and optional ratios for supported companies.

Company coverage, history, fields, request limits, and availability depend on the SimFin subscription plan.

## See also

[Connector configuration](simfin/configuration_simfin.md)

[Graphical configuration](simfin/graphical_configuration_simfin.md)

[Adapter initialization](simfin/adapter_initialization_simfin.md)
