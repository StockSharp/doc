# Adapter initialization: 5paisa Xstream

The following code shows how to initialize [FivePaisaMessageAdapter](xref:StockSharp.FivePaisa.FivePaisaMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FivePaisaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<value>".ToSecureString(),
	AppKey = "<value>",
	ClientCode = "<value>",
	AlgoId = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
