> [!NOTE]
> QUOINEX has been renamed to Liquid, which shut down in 2022. This documentation is preserved for historical reference.

# Adapter initialization Quoinex

The code below demonstrates how to initialize the [QuoinexMessageAdapter](xref:StockSharp.Quoinex.QuoinexMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new QuoinexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
