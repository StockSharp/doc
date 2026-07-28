# Inicialización del adaptador: AscendEX

El siguiente código inicializa [AscendExMessageAdapter](xref:StockSharp.AscendEx.AscendExMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new AscendExMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave API>".To<SecureString>(),
	Secret = "<Su secreto API>".To<SecureString>(),
	AccountGroup = 0,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_ascendex.md).

## Véase también

[Configuración del conector](configuration_ascendex.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
