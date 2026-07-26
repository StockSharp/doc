> [!CAUTION]
> **The PrizmBit exchange and its API are no longer available. The connector does not work; the documentation is retained for reference only.**

# Adapter initialization PrizmBit

The code below demonstrates how to initialize the [PrizmBitMessageAdapter](xref:StockSharp.PrizmBit.PrizmBitMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new PrizmBitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
