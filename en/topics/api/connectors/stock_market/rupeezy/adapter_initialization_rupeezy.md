# Adapter initialization: Rupeezy

The following code initializes [RupeezyMessageAdapter](xref:StockSharp.Rupeezy.RupeezyMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new RupeezyMessageAdapter(Connector.TransactionIdGenerator)
{
	ApplicationId = "<id>",
	ApiKey = "<key>".ToSecureString(),
	AuthCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_rupeezy.md) page.

## See also

[Connector configuration](configuration_rupeezy.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
