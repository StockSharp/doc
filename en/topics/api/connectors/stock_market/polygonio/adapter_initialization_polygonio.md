# Adapter initialization PolygonIO

The code below demonstrates how to initialize the [PolygonIOMessageAdapter](xref:StockSharp.PolygonIO.PolygonIOMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new PolygonIOMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your Token>".To<SecureString>(),
	ConnectionType = PolygonIOConnectionTypes.History, // connection for REST data sources
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
			
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
