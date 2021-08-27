# Adapter initialization Bitalong

The code below demonstrates how to initialize the [BitalongMessageAdapter](xref:StockSharp.Bitalong.BitalongMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
            Connector Connector = new Connector();				
            ...				
            var messageAdapter = new BitalongMessageAdapter(Connector.TransactionIdGenerator)
            {
                Key = "<Your API Key>".To<SecureString>(),
                Secret = "<Your API Secret>".To<SecureString>(),
            };
            Connector.Adapter.InnerAdapters.Add(messageAdapter);
            ...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
