# Inicialización del adaptador: Jainam

El siguiente código inicializa [JainamMessageAdapter](xref:StockSharp.Jainam.JainamMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new JainamMessageAdapter(Connector.TransactionIdGenerator)
{
	UserId = "<id>",
	AppCode = "<id>",
	ApiSecret = "<secret>".ToSecureString(),
	AuthCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_jainam.md).

## Véase también

[Configuración del conector](configuration_jainam.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
