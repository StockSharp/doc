# Adapter initialization Blackwood (Fusion)

The code below demonstrates how to initialize the [BlackwoodMessageAdapter](xref:StockSharp.Blackwood.BlackwoodMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

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

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
