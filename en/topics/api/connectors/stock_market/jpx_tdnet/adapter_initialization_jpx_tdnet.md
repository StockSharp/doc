# Adapter initialization: JPX TDnet

The following code initializes [JpxTdnetMessageAdapter](xref:StockSharp.JpxTdnet.JpxTdnetMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new JpxTdnetMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_jpx_tdnet.md) page.

## See also

[Connector configuration](configuration_jpx_tdnet.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
