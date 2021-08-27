# Adapter initialization Oanda

The code below demonstrates how to initialize the [OandaMessageAdapter](xref:StockSharp.Oanda.OandaMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new OandaMessageAdapter(Connector.TransactionIdGenerator)
{
    Token = "<Your Token>".To<SecureString>(),
    Server = OandaServers.Practice, // Demo
    //Server = OandaServers.Real,   // Real
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
