# Adapter initialization: Open DART

The following code initializes [OpenDartMessageAdapter](xref:StockSharp.OpenDart.OpenDartMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new OpenDartMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_open_dart.md) page.

## See also

[Connector configuration](configuration_open_dart.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
