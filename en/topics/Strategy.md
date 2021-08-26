# Strategies

The [S\#](StockSharpAbout.md) library contains a mechanism of multithreaded trading strategies writing described by the [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html) class. The advantages of this approach are as follows:

1. The ability to use the event model, to handle in parallel tens (hundreds, depending on the computer performance and the complexity of the algorithm) instruments with different parameters: time frames, volumes, etc. For more details see the 

   [Creating strategies](StrategyAction.md)

    section. 
2. The ability to use the iteration model. If you want a simple strategy implementation, that not critical to the execution speed. For more details see 

   [Iteration model](StrategyCreate.md)

   . 
3. Automatic metric of orders and trades. The ability to obtain the calculated values of 

   [Slippage](Slippage.md)

   , 

   [Profit\-loss](PnL.md)

   [Position](Position.md)

    and 

   [Latency](Latency.md)
4. [Commission](Commissions.md)

    calculating when trading. 
5. Complex strategies creating using the 

   [Child strategies](StrategyChilds.md)

    approach. 
6. The market orders emulation (where it not supported) through the 

   [Quoting](StrategyQuoting.md)

    strategy. 
7. Built\-in strategies connecting, such as 

   [Take\-profit and Stop\-loss](StrategyProtective.md)

   . 
8. The export to 

   [Excel](https://en.wikipedia.org/wiki/Excel)

    or 

   [Xml](https://en.wikipedia.org/wiki/XML)

    files of reports about strategy operation statistics. For more details see in the 

   [Reports](StrategyReports.md)

    section. 
9. The isolation of the trading logic from the system that allows to transfer strategy in a compiled code between computers. 
10. Flexible 

    [Logging](Logging.md)

    . 
11. [Monitoring of work](LoggingMonitorWindow.md)

     by using the graphical window. 
12. The 

    [simulation](StrategyTesting.md)

     on historical (backtesting), real\-time (without actual registration orders \- “paper trading”) and completely random data. 
13. The 

    [settings saving](StrategyStorage.md)

     to a file for the work recovery after the algorithm reboot, as well as 

    [loading of previous operations](StrategyOrdersLoad.md)

    . 
