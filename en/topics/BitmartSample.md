# Adapter initialization Bitmart

The code below demonstrates how to initialize the [BitmartMessageAdapter](xref:StockSharp.Bitmart.BitmartMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new BitmartMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
