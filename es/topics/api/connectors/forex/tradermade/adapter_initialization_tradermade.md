# Inicialización del adaptador: TraderMade

El siguiente código inicializa [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new TraderMadeMessageAdapter(connector.TransactionIdGenerator)
{
	RestKey = "<Su clave de API REST>".To<SecureString>(),
	StreamingKey = "<Su clave de API de transmisión>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los valores de acceso y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_tradermade.md).

## Véase también

[Configuración del conector](configuration_tradermade.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
