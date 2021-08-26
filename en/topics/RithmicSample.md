# Adapter initialization Rithmic

The code below demonstrates how to initialize the [RithmicMessageAdapter](../api/StockSharp.Rithmic.RithmicMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new RithmicMessageAdapter(Connector.TransactionIdGenerator)
{
    UserName = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
    CertFile = "<Path to certificate file>",
    Server = RithmicServers.Real,
    //Server = RithmicServers.Test,
    //Server = RithmicServers.Simulator,  
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
