# Inicialización del adaptador: IIFL

El siguiente código inicializa [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new IIFLMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave de API>".To<SecureString>(),
	Secret = "<Su secreto de API>".To<SecureString>(),
	ClientId = "<Su identificador de cliente>",
	AuthorizationCode = "<Su código de autorización>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los valores de acceso y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_iifl.md).

## Véase también

[Configuración del conector](configuration_iifl.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
