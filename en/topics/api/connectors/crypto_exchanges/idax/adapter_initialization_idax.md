> [!WARNING]
> This exchange has permanently shut down (November 2019 — shut down). This connector is no longer operational. Documentation is preserved for historical reference.

# Idax adapter initialization

The code below demonstrates how to initialize the [IdaxMessageAdapter](xref:StockSharp.Idax.IdaxMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new IdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
