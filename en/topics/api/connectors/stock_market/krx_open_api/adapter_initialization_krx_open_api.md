# Adapter initialization: KRX Open API

The following code initializes [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KrxOpenApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_krx_open_api.md) page.

## See also

[Connector configuration](configuration_krx_open_api.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
