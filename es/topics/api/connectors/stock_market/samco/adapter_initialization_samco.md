# Inicialización del adaptador: Samco

El siguiente código inicializa [SamcoMessageAdapter](xref:StockSharp.Samco.SamcoMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SamcoMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave de API>".To<SecureString>(),
	Secret = "<Su secreto de API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los valores de acceso y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_samco.md).

## Véase también

[Configuración del conector](configuration_samco.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
