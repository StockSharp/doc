# Adapter initialization: DNSE

The following code initializes [DnseMessageAdapter](xref:StockSharp.Dnse.DnseMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DnseMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	TradingToken = "<token>".ToSecureString(),
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_dnse.md) page.

## See also

[Connector configuration](configuration_dnse.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
