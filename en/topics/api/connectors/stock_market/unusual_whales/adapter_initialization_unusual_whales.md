# Adapter initialization: Unusual Whales

The following code initializes [UnusualWhalesMessageAdapter](xref:StockSharp.UnusualWhales.UnusualWhalesMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new UnusualWhalesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_unusual_whales.md) page.

## See also

[Connector configuration](configuration_unusual_whales.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
