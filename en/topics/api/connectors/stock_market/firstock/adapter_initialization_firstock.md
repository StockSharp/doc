# Adapter initialization: Firstock

The following code initializes [FirstockMessageAdapter](xref:StockSharp.Firstock.FirstockMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FirstockMessageAdapter(Connector.TransactionIdGenerator)
{
	UserId = "<id>",
	Password = "<secret>".ToSecureString(),
	OneTimePassword = "<secret>".ToSecureString(),
	VendorCode = "<id>",
	ApiKey = "<key>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_firstock.md) page.

## See also

[Connector configuration](configuration_firstock.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
