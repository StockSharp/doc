# Adapter initialization: comdirect

The following code initializes [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ComdirectMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_comdirect.md) page.

## See also

[Connector configuration](configuration_comdirect.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
