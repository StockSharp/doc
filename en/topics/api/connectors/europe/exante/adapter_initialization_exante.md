# Adapter initialization: EXANTE

The following code initializes [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ExanteMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
	SummaryCurrency = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_exante.md) page.

## See also

[Connector configuration](configuration_exante.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
