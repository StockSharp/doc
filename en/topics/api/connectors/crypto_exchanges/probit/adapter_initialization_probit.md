# Initialization of ProBit Global Adapter

The code below demonstrates how to initialize the [ProBitMessageAdapter](xref:StockSharp.ProBit.ProBitMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ProBitMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your OAuth client ID>".To<SecureString>(),
	Secret = "<Your OAuth client secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Omit `Key` and `Secret` when only public market data is required.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
