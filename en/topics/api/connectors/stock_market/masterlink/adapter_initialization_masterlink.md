# Adapter initialization: MasterLink

The following code initializes [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MasterLinkMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	CertificatePath = "<id>",
	CertificatePassword = "<secret>".ToSecureString(),
	NodePath = "<id>",
	GatewayDirectory = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_masterlink.md) page.

## See also

[Connector configuration](configuration_masterlink.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
