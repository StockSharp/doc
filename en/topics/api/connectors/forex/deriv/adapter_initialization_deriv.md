# Adapter initialization: Deriv

The following code initializes [DerivMessageAdapter](xref:StockSharp.Deriv.DerivMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DerivMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AppId = "<app-id>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the token and application identifier issued for the selected demo or real account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
