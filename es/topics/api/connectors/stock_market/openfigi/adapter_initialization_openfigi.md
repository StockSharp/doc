# Inicialización del adaptador: OpenFIGI

El siguiente código inicializa [OpenFigiMessageAdapter](xref:StockSharp.OpenFigi.OpenFigiMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new OpenFigiMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave de API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los valores de acceso y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_openfigi.md).

## Véase también

[Configuración del conector](configuration_openfigi.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
