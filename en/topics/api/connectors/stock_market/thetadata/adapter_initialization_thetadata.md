# Adapter initialization ThetaData

The code below demonstrates how to initialize the [ThetaDataMessageAdapter](xref:StockSharp.ThetaData.ThetaDataMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ThetaDataMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
