> [!CAUTION]
> **OKCoin trading services were disabled following the platform's transition to OKX. This connector no longer works; the documentation is retained for reference only.**

# Adapter initialization OKCoin

The code below demonstrates how to initialize the [OkcoinMessageAdapter](xref:StockSharp.Okcoin.OkcoinMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new OkcoinMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
