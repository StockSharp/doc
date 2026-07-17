# Adapter initialization: SinoPac Shioaji

The following code shows how to initialize [ShioajiMessageAdapter](xref:StockSharp.Shioaji.ShioajiMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ShioajiMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<value>".ToSecureString(),
	Secret = "<value>".ToSecureString(),
	Address = "<value>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
