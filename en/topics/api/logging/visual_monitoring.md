# Visual monitoring

To simplify monitoring, you can use the special [Monitor](xref:StockSharp.Xaml.Monitor) component. See also [Visual logging components](../graphical_user_interface/logging.md).

![GUI LogControl](../../../images/gui_logcontrol.png)

This window allows you to display messages from all [ILogSource](xref:Ecng.Logging.ILogSource): 

- strategies ([Strategy](xref:StockSharp.Algo.Strategies.Strategy));
- connectors ([IConnector](xref:StockSharp.BusinessEntities.IConnector));
- own [ILogSource](xref:Ecng.Logging.ILogSource) implementations (for example, the main window in the algorithm).

The nesting of sources is shown in the form of a tree. Each parent node contains messages from all nested sources and so on down to the lowest level. For connectors this is also useful when using [BasketTrader](../connectors.md). Similarly, the same nesting can be arranged for your own algorithm by implementing the [ILogSource.Parent](xref:Ecng.Logging.ILogSource.Parent) property.

## Using Monitor

1. First, you need to create a window and add the component.
2. Then, the created window must be added to your [LogManager](xref:Ecng.Logging.LogManager) through the [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener):

   ```cs
   _logManager.Listeners.Add(new GuiLogListener(monitor));
   ```
3. Thereafter all sources [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) (strategies, connectors, etc.) will send messages to the [Monitor](xref:StockSharp.Xaml.Monitor).

## Recommended content

[Visual logging components](../graphical_user_interface/logging.md)
