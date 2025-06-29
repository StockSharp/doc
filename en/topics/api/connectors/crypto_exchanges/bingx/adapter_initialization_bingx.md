# Adapter initialization BingX

The code below shows how to initialize [BingXMessageAdapter](xref:StockSharp.BingX.BingXMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
                        Connector Connector = new Connector();
                        ...
                        var messageAdapter = new BingXMessageAdapter(Connector.TransactionIdGenerator)
                        {
                                Key = "<Your API Key>".To<SecureString>(),
                                Secret = "<Your API Secret>".To<SecureString>(),
                        };
                        Connector.Adapter.InnerAdapters.Add(messageAdapter);
                        ...
```

## See also

[Connection Settings Window](../../../graphical_user_interface/connection_settings_window.md)
