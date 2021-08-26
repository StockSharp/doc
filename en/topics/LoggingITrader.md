# IConnector logging

The [IConnector](../api/StockSharp.BusinessEntities.IConnector.html) objects similar [strategies](LoggingStrategy.md) also implement the [ILogSource](../api/StockSharp.Logging.ILogSource.html) interface. Therefore, from the [IConnector](../api/StockSharp.BusinessEntities.IConnector.html) object you can receive messages through the [LogManager](../api/StockSharp.Logging.LogManager.html) by all means that are available for strategies. 

### IConnector logging example

IConnector logging example

1. First, you need to create a log manager: 

   ```cs
   ...
   private readonly Connector \_connector \= new Connector();
   private readonly LogManager \_logManager \= new LogManager();
   ...
   				
   				
   ```
2. Then you need to create a file logger, and to add it to the [LogManager.Listeners](../api/StockSharp.Logging.LogManager.Listeners.html): 

   ```cs
   \_logManager.Listeners.Add(new FileLogListener());
   ```
3. The final step is to add the [InteractiveBrokersTrader](../api/StockSharp.InteractiveBrokers.InteractiveBrokersTrader.html) in [LogManager.Sources](../api/StockSharp.Logging.LogManager.Sources.html): 

   ```cs
   \_logManager.Sources.Add(\_connector);
   ```
4. As a result, the program will display messages as shown below after the start: 

   ```none
   18:43:15 \| Info  \| RithmicTrader
   18:43:15 \| Debug \| RithmicTrader     \| ReadPortfolios()
   18:43:15 \| Debug \| RithmicTrader      \| OnProcessPortfolios()
   18:43:15 \| Debug \| RithmicTrader      \| 41469\|15152,43\|15530,8\|
   18:43:15 \| Debug \| RithmicTrader     \| ReadSecurities()
   18:43:15 \| Debug \| RithmicTrader      \| OnProcessSecurities
   18:43:15 \| Debug \| RithmicTrader      \| 291\|ESU5\|
   ```
