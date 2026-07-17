# Moomoo adapter initialization

The following code demonstrates how to initialize [MoomooMessageAdapter](xref:StockSharp.Moomoo.MoomooMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MoomooMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Loopback, 11111),
	Password = "<Password>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
