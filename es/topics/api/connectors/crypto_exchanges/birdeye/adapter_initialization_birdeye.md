# Inicialización del adaptador: Birdeye

El siguiente código inicializa [BirdeyeMessageAdapter](xref:StockSharp.Birdeye.BirdeyeMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new BirdeyeMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Su token de acceso>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_birdeye.md).

## Véase también

[Configuración del conector](configuration_birdeye.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
