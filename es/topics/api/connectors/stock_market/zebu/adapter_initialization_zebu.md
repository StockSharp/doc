# Inicialización del adaptador: Zebu

El siguiente código inicializa [ZebuMessageAdapter](xref:StockSharp.Zebu.ZebuMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ZebuMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	AuthorizationCode = "<code>".ToSecureString(),
	UserId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_zebu.md).

## Véase también

[Configuración del conector](configuration_zebu.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
