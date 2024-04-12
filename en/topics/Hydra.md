# Hydra

![hydra main](../images/hydra_main.png)

The [Hydra]() program (code\-named Hydra) is intended for automatic download of market data (instruments, candles, tick trades and order books) from various sources and storing them in a local storage. Data can be stored in two formats: in the special binary format [Hydra]() (BIN), which provides the maximum compression rate, or in the plain text format CSV, which is useful in analyzing the data in other programs. Later the stored information is available for trading strategies (for more information about strategies testing see the [Backtesting](api/testing.md) section). Access to data can be obtained directly through the [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) use (for more details see the [Market\-data storage](api/market_data_storage.md)), or through the usual export to formats such as [Excel](https://en.wikipedia.org/wiki/Excel), xml or txt (for more details see the [Installation and operation](hydra/installing_hydra.md) section). 

At the same time, [Hydra]() can use both historical and real\-time data sources (for example, connection to the [OpenECry](api/connectors/stock_market/openecry.md) or [Rithmic](api/connectors/stock_market/rithmic.md) for the order books getting). This is possible through the use of the extensible (plug\-in) sources model. 

Plugin model allows you to develop your own sources. About the development of own sources and installing them in the [Hydra]() program, see the [Creating source](hydra/create_new_souce.md) section. 
