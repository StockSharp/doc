# Adapter initialization: Alice Blue

The following code shows how to initialize [AliceBlueMessageAdapter](xref:StockSharp.AliceBlue.AliceBlueMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new AliceBlueMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<value>".ToSecureString(),
	UserId = "<value>",
	ClientId = "<value>",
	DeviceId = "<value>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
