# GLEIF

The **GLEIF connector** connects StockSharp to a financial-data and reference-information service. It translates provider-specific data into the unified StockSharp message model, so applications can use the same subscriptions and workflows across different data sources.

## Key capabilities

- Typical coverage: equities and issuer reference data.
- Instrument discovery and provider reference data.
- Provider-supported market, company, filing, disclosure, and reference data.
- This adapter is intended for data access and does not route orders.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector for security master data, disclosure monitoring, issuer research, compliance workflows, and historical analysis.

Available instruments, data depth, trading permissions, rate limits, and service availability are controlled by GLEIF and by the connected account or API plan.

## See also

[Connector configuration](gleif/configuration_gleif.md)

[Graphical configuration](gleif/graphical_configuration_gleif.md)

[Adapter initialization](gleif/adapter_initialization_gleif.md)
