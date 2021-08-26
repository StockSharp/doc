# Adapter initialization Blackwood (Fusion)

The code below demonstrates how to initialize the [BlackwoodMessageAdapter](../api/StockSharp.Blackwood.BlackwoodMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector = new Connector();				
...				
var address = "<Address>".To<IPAddress>();
var messageAdapter = new BlackwoodMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
    ExecutionAddress = new IPEndPoint(address, BlackwoodAddresses.ExecutionPort),
    MarketDataAddress = new IPEndPoint(address, BlackwoodAddresses.MarketDataPort),
    HistoricalDataAddress = new IPEndPoint(address, BlackwoodAddresses.HistoricalDataPort)
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
