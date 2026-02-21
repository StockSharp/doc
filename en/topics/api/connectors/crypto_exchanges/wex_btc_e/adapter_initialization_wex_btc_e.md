> [!WARNING]
> This exchange has permanently shut down (July 2017 — seized). This connector is no longer operational. Documentation is preserved for historical reference.

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

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
