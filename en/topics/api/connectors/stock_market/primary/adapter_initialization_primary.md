# Adapter initialization: Primary

The following code initializes [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new PrimaryMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	IsDemo = true,
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_primary.md) page.

## See also

[Connector configuration](configuration_primary.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
