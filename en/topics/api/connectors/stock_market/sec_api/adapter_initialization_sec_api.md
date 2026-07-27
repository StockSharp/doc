# Adapter initialization: SEC API

The following code initializes [SecApiMessageAdapter](xref:StockSharp.SecApi.SecApiMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SecApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_sec_api.md) page.

## See also

[Connector configuration](configuration_sec_api.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
