> [!WARNING]
> This exchange has permanently shut down (~2021 — shut down). This connector is no longer operational. Documentation is preserved for historical reference.

# Adapter initialization BW

The code below demonstrates how to initialize the [BWMessageAdapter](xref:StockSharp.BW.BWMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BWMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
