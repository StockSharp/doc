> [!CAUTION]
> **The BitMEX exchange shut down on September 23, 2026. The connector no longer works; the documentation is retained for reference only.**

# Adapter initialization BitMEX

The code below demonstrates how to initialize the [BitmexMessageAdapter](xref:StockSharp.Bitmex.BitmexMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BitmexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
