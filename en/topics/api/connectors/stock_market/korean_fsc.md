# Korean FSC

The **Korean FSC connector** connects StockSharp to a professional market-data and analytics service. It translates provider-specific data and operations into the unified StockSharp message model, so applications can use the same subscriptions and workflows across different venues.

## Key capabilities

- Typical coverage: equities.
- Instrument discovery and provider reference data.
- Provider-supported market, company, filing, disclosure, and reference data.
- Market data supported by the adapter: Level 1 quotes and candles.
- Historical data requests for charting, analysis, and backtesting.
- This adapter is intended for data access and does not route orders.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to feed charts, market-data storage, analytics, research workflows, and strategy testing with provider data.

Available instruments, data depth, trading permissions, rate limits, and service availability are controlled by Korean FSC and by the connected account or API plan.

## See also

[Connector configuration](korean_fsc/configuration_korean_fsc.md)

[Graphical configuration](korean_fsc/graphical_configuration_korean_fsc.md)

[Adapter initialization](korean_fsc/adapter_initialization_korean_fsc.md)
