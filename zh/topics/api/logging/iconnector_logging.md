# IConnector 日志记录

[IConnector](xref:StockSharp.BusinessEntities.IConnector) 对象类似的[策略](strategy_logging.md)也实现了 [ILogSource](xref:Ecng.Logging.ILogSource) 接口。因此，通过 [IConnector](xref:StockSharp.BusinessEntities.IConnector) 对象，你可以通过 [LogManager](xref:Ecng.Logging.LogManager) 以策略所提供的所有方式接收消息。

## IConnector 日志记录示例

1. 首先，你需要创建一个日志管理器：

   ```cs
   ...
   private readonly Connector _connector = new Connector();
   private readonly LogManager _logManager = new LogManager();
   ...
   				
   				
   ```
2. 然后你需要创建一个文件记录器，并将其添加到 [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners)：

   ```cs
   _logManager.Listeners.Add(new FileLogListener());
   ```
3. 最后一步是将 [Connector](xref:StockSharp.Algo.Connector) 添加到 [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) 上：

   ```cs
   _logManager.Sources.Add(_connector);
   ```
4. 因此，程序在启动后将显示如下所示的消息：

   ```none
   18:43:15 | Info  | RithmicTrader
   18:43:15 | Debug | RithmicTrader     | ReadPortfolios()
   18:43:15 | Debug | RithmicTrader      | OnProcessPortfolios()
   18:43:15 | Debug | RithmicTrader      | 41469|15152,43|15530,8|
   18:43:15 | Debug | RithmicTrader     | ReadSecurities()
   18:43:15 | Debug | RithmicTrader      | OnProcessSecurities
   18:43:15 | Debug | RithmicTrader      | 291|ESU5|
   ```
