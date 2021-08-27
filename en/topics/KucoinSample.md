# Adapter initialization Kucoin

The code below demonstrates how to initialize the [KucoinMessageAdapter](xref:StockSharp.Kucoin.KucoinMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
            Connector Connector = new Connector();				
            ...				
            var messageAdapter = new KucoinMessageAdapter(Connector.TransactionIdGenerator)
            {
                Key = "<Your API Key>".To<SecureString>(),
                Secret = "<Your API Secret>".To<SecureString>(),
            };
            Connector.Adapter.InnerAdapters.Add(messageAdapter);
            ...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
