# Adapter initialization Nasdaq Data Link

The code below demonstrates how to initialize the [NasdaqDataLinkMessageAdapter](xref:StockSharp.NasdaqDataLink.NasdaqDataLinkMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new NasdaqDataLinkMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
