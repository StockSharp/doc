# Registo de IConnector

Os objectos [IConnector](xref:StockSharp.BusinessEntities.IConnector), tal como as [estratégias](strategy_logging.md), também implementam a interface [ILogSource](xref:Ecng.Logging.ILogSource). Por isso, a partir do objecto [IConnector](xref:StockSharp.BusinessEntities.IConnector) pode receber mensagens através do [LogManager](xref:Ecng.Logging.LogManager), por todos os meios disponíveis para estratégias.

## Exemplo de registo de IConnector

1. Primeiro, tem de criar um gestor de registos:

   ```cs
   ...
   private readonly Connector _connector = new Connector();
   private readonly LogManager _logManager = new LogManager();
   ...
   				
   				
   ```
2. Depois, tem de criar um registador de ficheiro e adicioná-lo a [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners):

   ```cs
   _logManager.Listeners.Add(new FileLogListener());
   ```
3. O passo final é adicionar o [Connector](xref:StockSharp.Algo.Connector) a [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources):

   ```cs
   _logManager.Sources.Add(_connector);
   ```
4. Como resultado, o programa apresentará mensagens como mostrado abaixo após o arranque:

   ```none
   18:43:15 | Info  | RithmicTrader
   18:43:15 | Debug | RithmicTrader     | ReadPortfolios()
   18:43:15 | Debug | RithmicTrader      | OnProcessPortfolios()
   18:43:15 | Debug | RithmicTrader      | 41469|15152,43|15530,8|
   18:43:15 | Debug | RithmicTrader     | ReadSecurities()
   18:43:15 | Debug | RithmicTrader      | OnProcessSecurities
   18:43:15 | Debug | RithmicTrader      | 291|ESU5|
   ```
