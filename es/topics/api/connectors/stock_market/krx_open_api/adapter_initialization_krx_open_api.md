# Inicialización del adaptador: KRX Open API

El siguiente código inicializa [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KrxOpenApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_krx_open_api.md).

## Véase también

[Configuración del conector](configuration_krx_open_api.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
