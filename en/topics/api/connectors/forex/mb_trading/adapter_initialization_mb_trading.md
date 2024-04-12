# Adapter initialization MB Trading

The code below demonstrates how to initialize the [MBTradingMessageAdapter](xref:StockSharp.MBTrading.MBTradingMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
...	
var messageAdapter = new MBTradingMessageAdapter(Connector.TransactionIdGenerator)
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
