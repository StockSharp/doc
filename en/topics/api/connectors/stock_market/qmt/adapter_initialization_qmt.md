# Adapter initialization: QMT

The following code shows how to initialize [QmtMessageAdapter](xref:StockSharp.Qmt.QmtMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QmtMessageAdapter(Connector.TransactionIdGenerator)
{
	GatewayToken = "<value>".ToSecureString(),
	GatewayHost = "<value>",
	GatewayPort = 10,
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
