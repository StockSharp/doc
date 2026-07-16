# Lime Trader adapter initialization

The following code demonstrates how to initialize [LimeMessageAdapter](xref:StockSharp.Lime.LimeMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LimeMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Login>",
	Password = "<Password>".ToSecureString(),
	ClientId = "<Client ID>",
	ClientSecret = "<Client secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

