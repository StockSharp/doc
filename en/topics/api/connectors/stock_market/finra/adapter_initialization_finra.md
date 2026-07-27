# Adapter initialization: FINRA

The following code initializes [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinraMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_finra.md) page.

## See also

[Connector configuration](configuration_finra.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
