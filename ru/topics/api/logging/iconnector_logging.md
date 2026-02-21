# Логирование IConnector

Объекты [IConnector](xref:StockSharp.BusinessEntities.IConnector) аналогично [стратегиям](../strategies/logging.md), также реализуют интерфейс [ILogSource](xref:Ecng.Logging.ILogSource). Следовательно, от объекта [IConnector](xref:StockSharp.BusinessEntities.IConnector) можно получать сообщения через [LogManager](xref:Ecng.Logging.LogManager) всеми способами, что доступны стратегиям.

## Пример логирования IConnector

1. В самом начале необходимо создать менеджер логирования:

   ```cs
   ...
   private readonly Connector _connector = new Connector();
   private readonly LogManager _logManager = new LogManager();
   ...
   ```
2. Затем необходимо создать файловый логгер и добавить его в [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners):

   ```cs
   _logManager.Listeners.Add(new FileLogListener());
   ```
3. Заключительным этапом является добавление [Connector](xref:StockSharp.Algo.Connector) в [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources):

   ```cs
   _logManager.Sources.Add(_connector);
   ```
4. В итоге программа после запуска будет выводить сообщения, как показано ниже:

   ```none
   18:43:15 | Info  | Connector       | Connected
   18:43:15 | Debug | Connector       | StartExport()
   18:43:15 | Debug | Connector       | ReadPortfolios()
   18:43:15 | Debug | Connector       | OnProcessSecurities
   ```

Полный пример использования логирования смотрите в проекте *Samples/08_Misc/01_Logging*.
