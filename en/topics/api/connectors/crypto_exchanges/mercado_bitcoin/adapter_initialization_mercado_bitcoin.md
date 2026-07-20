# Initialization of Mercado Bitcoin Adapter

The code below demonstrates how to initialize the [MercadoBitcoinMessageAdapter](xref:StockSharp.MercadoBitcoin.MercadoBitcoinMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MercadoBitcoinMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your value>".To<SecureString>(),
	Secret = "<Your value>".To<SecureString>(),
	AccountId = "<Your value>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
