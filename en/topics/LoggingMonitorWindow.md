# Visual monitoring

For simplifying the monitoring of the work, you can use the special [MonitorWindow](../api/StockSharp.Xaml.MonitorWindow.html) window. See also [Visual logging components](GuiLogging.md). 

![GUI LogControl](../images/GUI_LogControl.png)

This window allows you to display messages from all [ILogSource](../api/StockSharp.Logging.ILogSource.html): 

- strategies (

  [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html)

  );
- connectors (

  [IConnector](../api/StockSharp.BusinessEntities.IConnector.html)

  );
- own 

  [ILogSource](../api/StockSharp.Logging.ILogSource.html)

   implementations (for example, the main window in the algorithm).

The nesting of sources is showing in the form of a tree. Each parent node contains messages of all nested and so on, until the lowest level. For strategies such hierarchy allows you to see [child strategies](StrategyChilds.md). For connectors it is also useful in the case of [BasketTrader](API_Connectors.md). using. Similarly, the same nesting can be arranged for your own algorithm, implementing the [ILogSource.Parent](../api/StockSharp.Logging.ILogSource.Parent.html) property. 

### MonitorWindow using

MonitorWindow using

1. First, you need to create a window:

   ```cs
   var monitor \= new MonitorWindow();
   monitor.Show();
   ```
2. Then, the created window must be added to your [LogManager](../api/StockSharp.Logging.LogManager.html) through the [GuiLogListener](../api/StockSharp.Xaml.GuiLogListener.html):

   ```cs
   \_logManager.Listeners.Add(new GuiLogListener(monitor));
   ```
3. Thereafter all sources [LogManager.Sources](../api/StockSharp.Logging.LogManager.Sources.html) (strategies, connectors, etc.) will send messages to the [MonitorWindow](../api/StockSharp.Xaml.MonitorWindow.html).

## Recommended content

[Visual logging components](GuiLogging.md)
