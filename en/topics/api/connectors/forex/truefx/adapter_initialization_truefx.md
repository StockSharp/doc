# Adapter initialization TrueFX

The code below demonstrates how to initialize the [TrueFXMessageAdapter](xref:StockSharp.TrueFX.TrueFXMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
...	
var messageAdapter = new TrueFXMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
};
...	
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
