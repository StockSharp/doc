# FINRA

The **FINRA connector** connects StockSharp to a financial-data and reference-information service. It translates provider-specific data into the unified StockSharp message model, so applications can use the same subscriptions and workflows across different data sources.

## Key capabilities

- Typical coverage: equities and issuer reference data.
- Instrument discovery and provider reference data.
- Provider-supported market, company, filing, disclosure, and reference data.
- Market data supported by the adapter: Level 1 quotes.
- Historical data requests for charting, analysis, and backtesting.
- This adapter is intended for data access and does not route orders.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector for security master data, disclosure monitoring, issuer research, compliance workflows, and historical analysis.

Available instruments, data depth, trading permissions, rate limits, and service availability are controlled by FINRA and by the connected account or API plan.

## See also

[Connector configuration](finra/configuration_finra.md)

[Graphical configuration](finra/graphical_configuration_finra.md)

[Adapter initialization](finra/adapter_initialization_finra.md)
