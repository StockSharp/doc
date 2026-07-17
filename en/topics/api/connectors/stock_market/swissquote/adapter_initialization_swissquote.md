# Adapter initialization: Swissquote OpenWealth

The following code shows how to initialize [SwissquoteMessageAdapter](xref:StockSharp.Swissquote.SwissquoteMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SwissquoteMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<value>".ToSecureString(),
	CustomerId = "<value>",
	SafekeepingAccountId = "<value>",
	CashAccountId = "<value>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
