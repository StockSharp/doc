# XBRL Filings

The **XBRL Filings connector** connects StockSharp to a financial-data and reference-information service. It translates provider-specific data into the unified StockSharp message model, so applications can use the same subscriptions and workflows across different data sources.

## Key capabilities

- Typical coverage: equities and issuer reference data.
- Instrument discovery and provider reference data.
- Provider-supported market, company, filing, disclosure, and reference data.
- Market data supported by the adapter: financial news and financial disclosures.
- Historical data requests for charting, analysis, and backtesting.
- This adapter is intended for data access and does not route orders.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector for security master data, disclosure monitoring, issuer research, compliance workflows, and historical analysis.

Available instruments, data depth, trading permissions, rate limits, and service availability are controlled by XBRL Filings and by the connected account or API plan.

## See also

[Connector configuration](xbrl_filings/configuration_xbrl_filings.md)

[Graphical configuration](xbrl_filings/graphical_configuration_xbrl_filings.md)

[Adapter initialization](xbrl_filings/adapter_initialization_xbrl_filings.md)
