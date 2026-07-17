# Adapter initialization: eToro

The following code shows how to initialize [EtoroMessageAdapter](xref:StockSharp.Etoro.EtoroMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EtoroMessageAdapter(Connector.TransactionIdGenerator)
{
	PublicApiKey = "<value>".ToSecureString(),
	UserKey = "<value>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
