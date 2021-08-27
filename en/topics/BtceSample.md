# Adapter initialization WEX (BTC\-e)

The code below demonstrates how to initialize the [BtceMessageAdapter](xref:StockSharp.Btce.BtceMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
            Connector Connector = new Connector();				
            ...				
            var messageAdapter = new BtceMessageAdapter(Connector.TransactionIdGenerator)
            {
                Key = "<Your API Key>".To<SecureString>(),
                Secret = "<Your API Secret>".To<SecureString>(),
            };
            Connector.Adapter.InnerAdapters.Add(messageAdapter);
            ...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
