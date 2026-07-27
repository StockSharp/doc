# Adapter initialization: Nuvama

The following code initializes [NuvamaMessageAdapter](xref:StockSharp.Nuvama.NuvamaMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new NuvamaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	RequestId = "<id>".ToSecureString(),
	AppIdKey = "<key>".ToSecureString(),
	PublicIpAddress = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_nuvama.md) page.

## See also

[Connector configuration](configuration_nuvama.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
