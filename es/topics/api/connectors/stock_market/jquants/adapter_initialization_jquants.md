# Inicialización del adaptador: J-Quants

El siguiente código inicializa [JQuantsMessageAdapter](xref:StockSharp.JQuants.JQuantsMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new JQuantsMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave de API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los valores de acceso y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_jquants.md).

## Véase también

[Configuración del conector](configuration_jquants.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
