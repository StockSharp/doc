# Adapter initialization: Bavest

The following code initializes [BavestMessageAdapter](xref:StockSharp.Bavest.BavestMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BavestMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_bavest.md) page.

## See also

[Connector configuration](configuration_bavest.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
