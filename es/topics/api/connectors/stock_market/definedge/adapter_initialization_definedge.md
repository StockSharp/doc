# Inicialización del adaptador: Definedge

El siguiente código inicializa [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DefinedgeMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	WebSocketToken = "<token>".ToSecureString(),
	UserId = "<id>",
	AccountId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_definedge.md).

## Véase también

[Configuración del conector](configuration_definedge.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
