# Логирование IConnector

Объекты [IConnector](../api/StockSharp.BusinessEntities.IConnector.html) аналогично [стратегиям](LoggingStrategy.md), также реализуют интерфейс [ILogSource](../api/StockSharp.Logging.ILogSource.html). Следовательно, от объекта [IConnector](../api/StockSharp.BusinessEntities.IConnector.html) можно получать сообщения через [LogManager](../api/StockSharp.Logging.LogManager.html) всеми способами, что доступны стратегиям. 

### Пример логирования IConnector

Пример логирования IConnector

1. В самом начале необходимо создать менеджер логирования: 

   ```cs
   ...
   private readonly Connector _connector = new Connector();
   private readonly LogManager _logManager = new LogManager();
   ...
   				
   				
   ```
2. Затем необходимо создать файловый логгер и добавить его в [LogManager.Listeners](../api/StockSharp.Logging.LogManager.Listeners.html): 

   ```cs
   _logManager.Listeners.Add(new FileLogListener());
   ```
3. Заключительным этапом является добавление [Connector](../api/StockSharp.Algo.Connector.html) в [LogManager.Sources](../api/StockSharp.Logging.LogManager.Sources.html): 

   ```cs
   _logManager.Sources.Add(_connector);
   ```
4. В итоге программа после запуска будет выводить сообщения, как показано ниже: 

   ```none
   18:43:15 | Info  | AlfaTrader      | AlfaDirect v.3.5.2.6
   18:43:15 | Debug | AlfaTrader      | StartExport()
   18:43:15 | Debug | AlfaWrapper     | ReadPortfolios()
   18:43:15 | Debug | AlfaTrader      | OnProcessPortfolios()
   18:43:15 | Debug | AlfaTrader      | 41469|15152,43|15530,8|
   18:43:15 | Debug | AlfaWrapper     | ReadSecurities()
   18:43:15 | Debug | AlfaTrader      | OnProcessSecurities
   18:43:15 | Debug | AlfaTrader      | 291|Лукойл а.о.|27.05.2011|4|LKOH|MICEX_SHR|RUR|0|0|MCX_SHR_LST|
   ```
