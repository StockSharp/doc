# Hydra

![hydra main](../images/hydra_main.png)

The [Hydra]() program is intended for automatic downloading of market data (instruments, candles, tick trades and order books) from various sources and for storing it locally. Data can be saved in two formats: the special binary format [Hydra]() (BIN), which provides the maximum compression rate, or the plain text format CSV, which is useful for analyzing the data in other programs. Later, the stored information becomes available for trading strategies (for more information about strategy testing see the [Backtesting](api/testing.md) section). Access to data can be obtained directly through the [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) (for more details see the [Market-data storage](api/market_data_storage.md)), or through the usual export to formats such as [Excel](https://en.wikipedia.org/wiki/Excel), XML or TXT (for more details see the [Installation and operation](hydra/installing_hydra.md) section).

At the same time, [Hydra]() can use both historical and real-time data sources (for example, connecting to [OpenECry](api/connectors/stock_market/openecry.md) or [Rithmic](api/connectors/stock_market/rithmic.md) to obtain order books). This is possible through the extensible plug-in source model.

The plug-in model lets you develop your own sources. For details on creating and installing custom sources in [Hydra](), see the [Creating source](hydra/create_new_source.md) section.
