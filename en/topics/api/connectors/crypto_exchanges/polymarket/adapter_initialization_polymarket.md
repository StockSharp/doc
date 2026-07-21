# Initialization of Polymarket Adapter

The code below demonstrates how to initialize the [PolymarketMessageAdapter](xref:StockSharp.Polymarket.PolymarketMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PolymarketMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Your value>",
	ApiSecret = "<Your value>".To<SecureString>(),
	Passphrase = "<Your value>".To<SecureString>(),
	SignerAddress = "<Your value>",
	FunderAddress = "<Your value>",
	PrivateKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
