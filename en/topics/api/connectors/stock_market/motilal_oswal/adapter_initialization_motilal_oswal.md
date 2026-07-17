# Adapter initialization: Motilal Oswal

The following code shows how to initialize [MotilalOswalMessageAdapter](xref:StockSharp.MotilalOswal.MotilalOswalMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MotilalOswalMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<value>".ToSecureString(),
	Secret = "<value>".ToSecureString(),
	Token = "<value>".ToSecureString(),
	AccessToken = "<value>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
