# Adapter initialization: GuruFocus

The following code initializes [GuruFocusMessageAdapter](xref:StockSharp.GuruFocus.GuruFocusMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GuruFocusMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_gurufocus.md) page.

## See also

[Connector configuration](configuration_gurufocus.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
