# Inicialización del adaptador: Finage

El siguiente código inicializa [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new FinageMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<Su clave de API>".To<SecureString>(),
	StreamingToken = "<Su token de transmisión>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los valores de acceso y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_finage.md).

## Véase también

[Configuración del conector](configuration_finage.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
