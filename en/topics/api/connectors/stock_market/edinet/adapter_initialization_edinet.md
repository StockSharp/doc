# Adapter initialization: EDINET

The following code initializes [EdinetMessageAdapter](xref:StockSharp.Edinet.EdinetMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EdinetMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_edinet.md) page.

## See also

[Connector configuration](configuration_edinet.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
