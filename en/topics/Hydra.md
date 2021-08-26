# S\#.Data

![hydra main](../images/hydra_main.png)

The [S\#.Data](Hydra.md) program (code\-named Hydra) is intended for automatic download of market data (instruments, candles, tick trades and order books) from various sources and storing them in a local storage. Data can be stored in two formats: in the special binary format [S\#.Data](Hydra.md) (BIN), which provides the maximum compression rate, or in the plain text format CSV, which is useful in analyzing the data in other programs. Later the stored information is available for trading strategies (for more information about strategies testing see the [Backtesting](StrategyTesting.md) section). Access to data can be obtained directly through the [StorageRegistry](../api/StockSharp.Algo.Storages.StorageRegistry.html) use (for more details see the [Market\-data storage](Storages.md)), or through the usual export to formats such as [Excel](https://en.wikipedia.org/wiki/Excel), xml or txt (for more details see the [Installation and operation](HydraUsing.md) section). 

At the same time, [S\#.Data](Hydra.md) can use both historical and real\-time data sources (for example, connection to the [OpenECry](OEC.md) or [Rithmic](Rithmic.md) for the order books getting). This is possible through the use of the extensible (plug\-in) sources model. 

Plugin model allows you to develop your own sources. About the development of own sources and installing them in the [S\#.Data](Hydra.md) program, see the [Creating source](HydraPlugins.md) section. 
