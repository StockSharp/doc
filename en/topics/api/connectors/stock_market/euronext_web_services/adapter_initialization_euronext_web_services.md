# Adapter initialization: Euronext Web Services

The following code initializes [EuronextWebServicesMessageAdapter](xref:StockSharp.EuronextWebServices.EuronextWebServicesMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EuronextWebServicesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_euronext_web_services.md) page.

## See also

[Connector configuration](configuration_euronext_web_services.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
