# Registro de IConnector

Los objetos [IConnector](xref:StockSharp.BusinessEntities.IConnector), de forma similar a las [estrategias](strategy_logging.md), también implementan la interfaz [ILogSource](xref:Ecng.Logging.ILogSource). Por lo tanto, desde el objeto [IConnector](xref:StockSharp.BusinessEntities.IConnector) puede recibir mensajes mediante [LogManager](xref:Ecng.Logging.LogManager) por todos los medios disponibles para las estrategias. 

## Ejemplo de registro de IConnector

1. Primero, necesita crear un administrador de registros:

   ```cs
   ...
   private readonly Connector _connector = new Connector();
   private readonly LogManager _logManager = new LogManager();
   ...
   				
   				
   ```
2. Después necesita crear un registrador de archivo y agregarlo a [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners):

   ```cs
   _logManager.Listeners.Add(new FileLogListener());
   ```
3. El paso final es agregar [Connector](xref:StockSharp.Algo.Connector) a [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources):

   ```cs
   _logManager.Sources.Add(_connector);
   ```
4. Como resultado, el programa mostrará mensajes como los siguientes después del inicio: 

   ```none
   18:43:15 | Info  | RithmicTrader
   18:43:15 | Debug | RithmicTrader     | ReadPortfolios()
   18:43:15 | Debug | RithmicTrader      | OnProcessPortfolios()
   18:43:15 | Debug | RithmicTrader      | 41469|15152,43|15530,8|
   18:43:15 | Debug | RithmicTrader     | ReadSecurities()
   18:43:15 | Debug | RithmicTrader      | OnProcessSecurities
   18:43:15 | Debug | RithmicTrader      | 291|ESU5|
   ```
