# Strategies

The [S\#](../api.md) library contains a mechanism of multithreaded trading strategies writing described by the [Strategy](xref:StockSharp.Algo.Strategies.Strategy) class. The advantages of this approach are as follows:

1. The ability to use the event model, to handle in parallel tens (hundreds, depending on the computer performance and the complexity of the algorithm) instruments with different parameters: time frames, volumes, etc. For more details see the [Creating strategies](strategies/creating_strategies.md) section. 
2. The ability to use the iteration model. If you want a simple strategy implementation, that not critical to the execution speed. For more details see [Iteration model](strategies/iteration_model.md). 
3. Automatic metric of orders and trades. The ability to obtain the calculated values of [Slippage](trading_algorithms/slippage.md), [Profit\-loss](trading_algorithms/profit_loss.md)[Position](trading_algorithms/position.md) and [Latency](trading_algorithms/latency.md)
4. [Commission](trading_algorithms/commission.md) calculating when trading. 
5. Complex strategies creating using the [Child strategies](strategies/child_strategies.md) approach. 
6. The market orders emulation (where it not supported) through the [Quoting](strategies/quoting.md) strategy. 
7. Built\-in strategies connecting, such as [Take\-profit and Stop\-loss](strategies/take_profit_and_stop_loss.md). 
8. The export to [Excel](https://en.wikipedia.org/wiki/Excel) or [Xml](https://en.wikipedia.org/wiki/XML) files of reports about strategy operation statistics. For more details see in the [Reports](strategies/reports.md) section. 
9. The isolation of the trading logic from the system that allows to transfer strategy in a compiled code between computers. 
10. Flexible [Logging](logging.md). 
11. [Monitoring of work](logging/visual_monitoring.md) by using the graphical window. 
12. The [simulation](testing.md) on historical (backtesting), real\-time (without actual registration orders \- “paper trading”) and completely random data. 
13. The [settings saving](strategies/settings_saving_and_loading.md) to a file for the work recovery after the algorithm reboot, as well as [loading of previous operations](strategies/orders_and_trades_loading.md). 
