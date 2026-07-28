# Adapter initialization: Settrade

The following code initializes [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SettradeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
	AppCode = "<Your application code>",
	BrokerId = "<Your broker identifier>",
	Account = "<Your account number>",
	Pin = "<Your trading PIN>".To<SecureString>(),
	AccountType = SettradeAccountTypes.Equity,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials, account type, and any other required properties described on the [Connector configuration](configuration_settrade.md) page.

## See also

[Connector configuration](configuration_settrade.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
