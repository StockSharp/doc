# Inicialización del adaptador: Coinalyze

El siguiente código inicializa [CoinalyzeMessageAdapter](xref:StockSharp.Coinalyze.CoinalyzeMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinalyzeMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Su token de acceso>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_coinalyze.md).

## Véase también

[Configuración del conector](configuration_coinalyze.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
