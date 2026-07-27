# Adapter initialization: TASE Data Hub

The following code initializes [TaseDataHubMessageAdapter](xref:StockSharp.TaseDataHub.TaseDataHubMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TaseDataHubMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_tase_data_hub.md) page.

## See also

[Connector configuration](configuration_tase_data_hub.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
