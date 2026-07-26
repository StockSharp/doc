> [!CAUTION]
> **The IEX Trading API used by this connector is no longer available. The connector does not work; the documentation is retained for reference only.**

# Adapter initialization IEX

The code below demonstrates how to initialize the [IEXMessageAdapter](xref:StockSharp.IEX.IEXMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new IEXMessageAdapter(Connector.TransactionIdGenerator)
{
	Token  = "<Your Token>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
