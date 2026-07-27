# Unusual Whales

The **Unusual Whales connector** connects StockSharp to a professional market-data and analytics service. It translates provider-specific data and operations into the unified StockSharp message model, so applications can use the same subscriptions and workflows across different venues.

## Key capabilities

- Typical coverage: equities and options.
- Instrument discovery and provider reference data.
- Provider-supported market, company, filing, disclosure, and reference data.
- Market data supported by the adapter: Level 1 quotes, candles, financial news and financial disclosures.
- Historical data requests for charting, analysis, and backtesting.
- Real-time subscriptions through the provider's streaming transport.
- This adapter is intended for data access and does not route orders.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to feed charts, market-data storage, analytics, research workflows, and strategy testing with provider data.

Available instruments, data depth, trading permissions, rate limits, and service availability are controlled by Unusual Whales and by the connected account or API plan.

## See also

[Connector configuration](unusual_whales/configuration_unusual_whales.md)

[Graphical configuration](unusual_whales/graphical_configuration_unusual_whales.md)

[Adapter initialization](unusual_whales/adapter_initialization_unusual_whales.md)
