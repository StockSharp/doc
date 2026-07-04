# IConnector-Logging

Die Objekte [IConnector](xref:StockSharp.BusinessEntities.IConnector) implementieren ähnlich wie [Strategien](strategy_logging.md) ebenfalls das Interface [ILogSource](xref:Ecng.Logging.ILogSource). Daher können Sie von einem [IConnector](xref:StockSharp.BusinessEntities.IConnector)-Objekt über den [LogManager](xref:Ecng.Logging.LogManager) Nachrichten mit denselben Mitteln empfangen, die auch für Strategien verfügbar sind.

## Beispiel für IConnector-Logging

1. Zuerst müssen Sie einen Logmanager erstellen:

   ```cs
   ...
   private readonly Connector _connector = new Connector();
   private readonly LogManager _logManager = new LogManager();
   ...
   				
   				
   ```
2. Danach müssen Sie einen Datei-Logger erstellen und ihn zu [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners) hinzufügen:

   ```cs
   _logManager.Listeners.Add(new FileLogListener());
   ```
3. Im letzten Schritt fügen Sie [Connector](xref:StockSharp.Algo.Connector) zu [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) hinzu:

   ```cs
   _logManager.Sources.Add(_connector);
   ```
4. Als Ergebnis zeigt das Programm nach dem Start Nachrichten wie unten dargestellt an:

   ```none
   18:43:15 | Info  | RithmicTrader
   18:43:15 | Debug | RithmicTrader     | ReadPortfolios()
   18:43:15 | Debug | RithmicTrader      | OnProcessPortfolios()
   18:43:15 | Debug | RithmicTrader      | 41469|15152,43|15530,8|
   18:43:15 | Debug | RithmicTrader     | ReadSecurities()
   18:43:15 | Debug | RithmicTrader      | OnProcessSecurities
   18:43:15 | Debug | RithmicTrader      | 291|ESU5|
   ```

