> [!WARNING]
> This exchange has permanently shut down (~2023 — effectively defunct). This connector is no longer operational. Documentation is preserved for historical reference.

# Adapter initialization Yobit

The code below demonstrates how to initialize the [YobitMessageAdapter](xref:StockSharp.Yobit.YobitMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new YobitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
