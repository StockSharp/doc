# Adapter initialization: Bigul

The following code initializes [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BigulMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientCode = "<id>",
	ApiKey = "<key>".ToSecureString(),
	ApiSecret = "<secret>".ToSecureString(),
	OneTimePassword = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	Source = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_bigul.md) page.

## See also

[Connector configuration](configuration_bigul.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
