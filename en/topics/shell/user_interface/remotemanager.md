# RemoteManager

The [RemoteManager]() tab allows you to enable remote control mode. To enable this mode, you must go to the user configuration menu.

![Shell RemoteManager 00](../../../images/shell_remotemanager_00.png)

In the window that appears, set your **login** and **password**.

![Shell RemoteManager 01](../../../images/shell_remotemanager_01.png)

Then you need to enable the **server mode**. 

![Shell RemoteManager 02](../../../images/shell_remotemanager_02.png)

You can now connect to Shell from another Shell.

To do this, you need to run **another Shell**. In it, go to the connection settings.

![Shell RemoteManager 03](../../../images/shell_remotemanager_03.png)

In the window that opens, set up the FIX connection.

![Shell RemoteManager 04](../../../images/shell_remotemanager_04.png)

Then press the Connect button.

![Shell RemoteManager 05](../../../images/shell_remotemanager_05.png)

When connected, all existing strategies in the Shell server will be available in the Shell client.

![Shell RemoteManager 06](../../../images/shell_remotemanager_06.png)

By clicking the Add button, you can add another strategy to trade.

![Shell RemoteManager 07](../../../images/shell_remotemanager_07.png)

Because the Shell client supports multiple servers, when adding a strategy you must select the server on the left. All strategies available on the server will appear on the right.

![Shell RemoteManager 08](../../../images/shell_remotemanager_08.png)

After adding a strategy, it will appear in the list of strategies.

![Shell RemoteManager 09](../../../images/shell_remotemanager_09.png)

When selecting a strategy, there will be tabs on the right with the strategy settings, as well as its statistics.

After changing the strategy settings, be sure to click the Apply changes button, otherwise the changes will not be applied to the strategy.

![Shell RemoteManager 10](../../../images/shell_remotemanager_10.png)

If the strategy has a command other than Start\/Stop, then to apply it you must set it in the next field.

![Shell RemoteManager 11](../../../images/shell_remotemanager_11.png)

And click the send command button.

To set your command in the strategy, you need to override the [Strategy.ApplyCommand](xref:StockSharp.Algo.Strategies.Strategy.ApplyCommand(StockSharp.Messages.CommandMessage))**(**[StockSharp.Messages.CommandMessage](xref:StockSharp.Messages.CommandMessage) cmdMsg **)** method.

```cs
public virtual void ApplyCommand(CommandMessage cmdMsg)
		
```

The [Strategy](xref:StockSharp.Algo.Strategies.Strategy) base class only controls the strategy start and stop.

## Recommended content

[Connections settings](../connections_settings.md)
