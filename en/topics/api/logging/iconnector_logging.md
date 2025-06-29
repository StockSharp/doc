# IConnector logging

The [IConnector](xref:StockSharp.BusinessEntities.IConnector) objects similar [strategies](strategy_logging.md) also implement the [ILogSource](xref:Ecng.Logging.ILogSource) interface. Therefore, from the [IConnector](xref:StockSharp.BusinessEntities.IConnector) object you can receive messages through the [LogManager](xref:Ecng.Logging.LogManager) by all means that are available for strategies. 

## IConnector logging example

1. First, you need to create a log manager: 

   ```cs
   ...
   private readonly Connector _connector = new Connector();
   private readonly LogManager _logManager = new LogManager();
   ...
   				
   				
   ```
2. Then you need to create a file logger, and to add it to the [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners): 

   ```cs
   _logManager.Listeners.Add(new FileLogListener());
   ```
3. The final step is to add the [Connector](xref:StockSharp.Algo.Connector) to [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources):

   ```cs
   _logManager.Sources.Add(_connector);
   ```
4. As a result, the program will display messages as shown below after the start: 

   ```none
   18:43:15 | Info  | RithmicTrader
   18:43:15 | Debug | RithmicTrader     | ReadPortfolios()
   18:43:15 | Debug | RithmicTrader      | OnProcessPortfolios()
   18:43:15 | Debug | RithmicTrader      | 41469|15152,43|15530,8|
   18:43:15 | Debug | RithmicTrader     | ReadSecurities()
   18:43:15 | Debug | RithmicTrader      | OnProcessSecurities
   18:43:15 | Debug | RithmicTrader      | 291|ESU5|
   ```
