# Hydra

![hydra main](../images/hydra_main.png)

**Hydra** automatically downloads market data from different sources and stores it locally. It supports instruments, candles, tick trades, order books, and other data types. Data can be stored in the special **Hydra** binary format (BIN), which provides high compression, or in CSV format for analysis in other programs.

Stored data can later be used by trading strategies. For strategy testing, see [Backtesting](api/testing.md). Data can be accessed directly through [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) (see [Market-data storage](api/market_data_storage.md)) or exported to formats such as [Excel](https://en.wikipedia.org/wiki/Excel), XML, or TXT (see [Installation and operation](hydra/installing_hydra.md)).

**Hydra** can work with both historical and real-time data sources, for example [OpenECry](api/connectors/stock_market/openecry.md) or [Rithmic](api/connectors/stock_market/rithmic.md) for order books. The plugin model also lets you create custom data sources. For details, see [Creating source](hydra/create_new_source.md).
