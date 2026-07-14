# Webull adapter initialization

The following code demonstrates how to initialize [WebullMessageAdapter](xref:StockSharp.Webull.WebullMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new WebullMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<application key>".ToSecureString(),
	Secret = "<application secret>".ToSecureString(),
	Token = "<access token>".ToSecureString(),
	Account = "<account identifier>",
	IsDemo = false,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

The `Token` and `Account` settings can be omitted when they are not required.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
