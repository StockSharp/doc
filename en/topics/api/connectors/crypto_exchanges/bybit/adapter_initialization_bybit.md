# ByBit Adapter Initialization

The code below demonstrates how to initialize the [ByBitMessageAdapter](xref:StockSharp.ByBit.ByBitMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ByBitMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

An alternative and more convenient way is to use the `AddAdapter<T>()` extension method:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<ByBitMessageAdapter>(a =>
{
	a.Key = "<Your API Key>".To<SecureString>();
	a.Secret = "<Your API Secret>".To<SecureString>();
});
```

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
