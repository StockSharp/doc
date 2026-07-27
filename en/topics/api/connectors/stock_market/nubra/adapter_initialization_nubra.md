# Adapter initialization: Nubra

The following code initializes [NubraMessageAdapter](xref:StockSharp.Nubra.NubraMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new NubraMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	DeviceId = "<id>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_nubra.md) page.

## See also

[Connector configuration](configuration_nubra.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
