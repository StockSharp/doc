# Inicialización del adaptador: Open DART

El siguiente código inicializa [OpenDartMessageAdapter](xref:StockSharp.OpenDart.OpenDartMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new OpenDartMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_open_dart.md).

## Véase también

[Configuración del conector](configuration_open_dart.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
