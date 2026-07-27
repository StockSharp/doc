# Adapter initialization: Mastertrust

The following code initializes [MastertrustMessageAdapter](xref:StockSharp.Mastertrust.MastertrustMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MastertrustMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<id>",
	OAuthClientSecret = "<secret>".ToSecureString(),
	AuthorizationCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_mastertrust.md) page.

## See also

[Connector configuration](configuration_mastertrust.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
