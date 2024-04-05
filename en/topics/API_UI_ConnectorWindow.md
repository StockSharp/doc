# Connection settings window

[ConnectorWindow](xref:StockSharp.Xaml.ConnectorWindow) \- a special window for configuring adapters for connecting a connector. 

![API GUI ConnectorWindow](../images/API_GUI_ConnectorWindow.png)

Here is the connection settings window. From the drop\-down list (opens with the '+' button), you need to select the necessary adapters and configure their properties in the properties window located on the right. 

This window should be called through the [Extensions.Configure](xref:StockSharp.Xaml.Extensions.Configure(StockSharp.Algo.Connector,System.Windows.Window))**(**[StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector) connector, [System.Windows.Window](xref:System.Windows.Window) owner**)**, extension method, into which the [Connector](xref:StockSharp.Algo.Connector) and the parent window are passed. If the configuration is successful, the [Extensions.Configure](xref:StockSharp.Xaml.Extensions.Configure(StockSharp.Algo.Connector,System.Windows.Window))**(**[StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector) connector, [System.Windows.Window](xref:System.Windows.Window) owner**)** extension method will return 'true'. Below is the code to call the connector connection settings window and save the settings to a file. 

```cs
		private void Setting_Click(object sender, RoutedEventArgs e)
		{
			if (_connector.Configure(this))
			{
				new JsonSerializer<SettingsStorage>().Serialize(_connector.Save(), _connectorFile);
			}
		}
	  				
```

> [!TIP]
> The connection correctness can be checked using the **Check** button.

The result of this window will be to create and add adapters to the list of *internal adapters* of the [Connector.Adapter](xref:StockSharp.Algo.Connector.Adapter) property. 

For more information about saving and loading connector settings, see [Save and load settings](API_Connectors_SaveConnectorSettings.md).
