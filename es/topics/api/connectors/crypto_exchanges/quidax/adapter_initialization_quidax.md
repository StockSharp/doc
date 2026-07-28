# Inicialización del adaptador: Quidax

El siguiente código inicializa [QuidaxMessageAdapter](xref:StockSharp.Quidax.QuidaxMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new QuidaxMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Su token de acceso>".To<SecureString>(),
	UserId = "<Su identificador de usuario>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_quidax.md).

## Véase también

[Configuración del conector](configuration_quidax.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
