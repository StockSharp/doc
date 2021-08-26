# Adapter initialization OpenECry

The code below demonstrates how to initialize the [OpenECryMessageAdapter](../api/StockSharp.OpenECry.OpenECryMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new OpenECryMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
    Address = "<Address>".To<EndPoint>(),
    EnableOECLogging = true,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
