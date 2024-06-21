# Market\-data storage

[S\#](../api.md) rovides functionality for data storage and loading. Data storage may be necessary in a few cases:

1. During working of trading strategy, when necessary to save the state (orders, positions, settings, etc.);
2. Market data saving from the trading terminal for algorithm testing in real time;
3. Data accumulation for analysis and data mining.

[S\#](../api.md) makes clear the saving mechanism due to high\-level access and the technical details hiding (for more details see [API](market_data_storage/api.md)). And also versatile, with possibility to expand the types of supported storages. 

## Recommended content

[Working with the API](market_data_storage/api.md)

[Working with Remote Storage](market_data_storage/remote.md)