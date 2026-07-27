# Adapter initialization: Tradernet

The following code initializes [TradernetMessageAdapter](xref:StockSharp.Tradernet.TradernetMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradernetMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_tradernet.md) page.

## See also

[Connector configuration](configuration_tradernet.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
