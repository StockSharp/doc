# Inicialización del adaptador: Euronext Web Services

El siguiente código inicializa [EuronextWebServicesMessageAdapter](xref:StockSharp.EuronextWebServices.EuronextWebServicesMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EuronextWebServicesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_euronext_web_services.md).

## Véase también

[Configuración del conector](configuration_euronext_web_services.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
