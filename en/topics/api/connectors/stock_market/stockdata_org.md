# StockData.org

The **StockData.org connector** connects StockSharp to a professional market-data and analytics service. It translates provider-specific data and operations into the unified StockSharp message model, so applications can use the same subscriptions and workflows across different venues.

## Key capabilities

- Typical coverage: equities.
- Instrument discovery and provider reference data.
- Provider-supported market, company, filing, disclosure, and reference data.
- Market data supported by the adapter: Level 1 quotes, candles, financial news and financial disclosures.
- Historical data requests for charting, analysis, and backtesting.
- This adapter is intended for data access and does not route orders.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to feed charts, market-data storage, analytics, research workflows, and strategy testing with provider data.

Available instruments, data depth, trading permissions, rate limits, and service availability are controlled by StockData.org and by the connected account or API plan.

## See also

[Connector configuration](stockdata_org/configuration_stockdata_org.md)

[Graphical configuration](stockdata_org/graphical_configuration_stockdata_org.md)

[Adapter initialization](stockdata_org/adapter_initialization_stockdata_org.md)
