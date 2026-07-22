# Initialization of CryptoQuant Adapter

The code below demonstrates how to initialize the [CryptoQuantMessageAdapter](xref:StockSharp.CryptoQuant.CryptoQuantMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CryptoQuantMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
