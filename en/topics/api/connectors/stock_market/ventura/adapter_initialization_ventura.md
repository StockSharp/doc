# Adapter initialization: Ventura

The following code initializes [VenturaMessageAdapter](xref:StockSharp.Ventura.VenturaMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new VenturaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	ClientId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_ventura.md) page.

## See also

[Connector configuration](configuration_ventura.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
