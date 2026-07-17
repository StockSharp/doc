# Adapter initialization: Shoonya

The following code shows how to initialize [ShoonyaMessageAdapter](xref:StockSharp.Shoonya.ShoonyaMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ShoonyaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<value>".ToSecureString(),
	UserId = "<value>",
	AccountId = "<value>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
