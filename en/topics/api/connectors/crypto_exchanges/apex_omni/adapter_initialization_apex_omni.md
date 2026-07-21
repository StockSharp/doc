# Initialization of ApeX Omni Adapter

The code below demonstrates how to initialize the [ApexOmniMessageAdapter](xref:StockSharp.ApexOmni.ApexOmniMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ApexOmniMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your value>".To<SecureString>(),
	Secret = "<Your value>".To<SecureString>(),
	Passphrase = "<Your value>".To<SecureString>(),
	Seeds = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
