# Adapter initialization: BCS

The following code initializes [BcsMessageAdapter](xref:StockSharp.Bcs.BcsMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BcsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_bcs.md) page.

## See also

[Connector configuration](configuration_bcs.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
