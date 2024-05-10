# Adapter initialization TradeOgre

The code below demonstrates how to initialize the [TradeOgreMessageAdapter](xref:StockSharp.TradeOgre.TradeOgreMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
            Connector Connector = new Connector();				
            ...				
            var messageAdapter = new TradeOgreMessageAdapter(Connector.TransactionIdGenerator)
            {
                Key = "<Your API Key>".To<SecureString>(),
                Secret = "<Your API Secret>".To<SecureString>(),
            };
            Connector.Adapter.InnerAdapters.Add(messageAdapter);
            ...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
