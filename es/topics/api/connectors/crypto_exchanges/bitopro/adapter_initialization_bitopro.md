# Inicialización del adaptador: BitoPro

El siguiente código inicializa [BitoProMessageAdapter](xref:StockSharp.BitoPro.BitoProMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new BitoProMessageAdapter(connector.TransactionIdGenerator)
{
	Email = "<Su correo electrónico>",
	Key = "<Su clave API>".To<SecureString>(),
	Secret = "<Su secreto API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_bitopro.md).

## Véase también

[Configuración del conector](configuration_bitopro.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
