# Adapter initialization: Quiver Quantitative

The following code initializes [QuiverQuantMessageAdapter](xref:StockSharp.QuiverQuant.QuiverQuantMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QuiverQuantMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_quiver_quant.md) page.

## See also

[Connector configuration](configuration_quiver_quant.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
