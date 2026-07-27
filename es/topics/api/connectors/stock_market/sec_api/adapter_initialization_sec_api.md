# Inicialización del adaptador: SEC API

El siguiente código inicializa [SecApiMessageAdapter](xref:StockSharp.SecApi.SecApiMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SecApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_sec_api.md).

## Véase también

[Configuración del conector](configuration_sec_api.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
