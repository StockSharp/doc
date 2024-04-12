# Adapter initialization GAIN Capital

The code below demonstrates how to initialize the [GainCapitalMessageAdapter](xref:StockSharp.GainCapital.GainCapitalMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GainCapitalMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
