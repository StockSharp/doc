> [!CAUTION]
> **The GDAX service is no longer available. Its successor, Coinbase Pro, has also been discontinued; use the current Coinbase connector for Coinbase. This connector does not work; the documentation is retained for reference only.**

# Adapter initialization GDAX

The code below demonstrates how to initialize the [GdaxMessageAdapter](xref:StockSharp.Gdax.GdaxMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new GdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
