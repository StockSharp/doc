# IConnector のロギング

[IConnector](xref:StockSharp.BusinessEntities.IConnector) オブジェクトは、[ストラテジー](strategy_logging.md) と同様に [ILogSource](xref:Ecng.Logging.ILogSource) インターフェイスも実装します。したがって、[IConnector](xref:StockSharp.BusinessEntities.IConnector) オブジェクトからは、ストラテジーで利用可能なすべての方法で [LogManager](xref:Ecng.Logging.LogManager) を通じてメッセージを受け取ることができます。 

## IConnector ロギングの例

1. まず、ログマネージャーを作成する必要があります。 

   ```cs
   ...
   private readonly Connector _connector = new Connector();
   private readonly LogManager _logManager = new LogManager();
   ...
   				
   				
   ```
2. 次に、ファイルロガーを作成し、[LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners) に追加する必要があります。 

   ```cs
   _logManager.Listeners.Add(new FileLogListener());
   ```
3. 最後の手順は、[Connector](xref:StockSharp.Algo.Connector) を [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) に追加することです。

   ```cs
   _logManager.Sources.Add(_connector);
   ```
4. その結果、プログラムの起動後に以下のようなメッセージが表示されます。 

   ```none
   18:43:15 | Info  | RithmicTrader
   18:43:15 | Debug | RithmicTrader     | ReadPortfolios()
   18:43:15 | Debug | RithmicTrader      | OnProcessPortfolios()
   18:43:15 | Debug | RithmicTrader      | 41469|15152,43|15530,8|
   18:43:15 | Debug | RithmicTrader     | ReadSecurities()
   18:43:15 | Debug | RithmicTrader      | OnProcessSecurities
   18:43:15 | Debug | RithmicTrader      | 291|ESU5|
   ```
