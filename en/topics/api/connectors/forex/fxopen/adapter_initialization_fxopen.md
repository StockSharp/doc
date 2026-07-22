# Adapter initialization: FXOpen TickTrader

The following code initializes [FXOpenMessageAdapter](xref:StockSharp.FXOpen.FXOpenMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FXOpenMessageAdapter(Connector.TransactionIdGenerator)
{
	WebApiId = "<web-api-id>",
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the token parameters issued for the selected live or demo account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
