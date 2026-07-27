# Inicialización del adaptador: Rupeezy

El siguiente código inicializa [RupeezyMessageAdapter](xref:StockSharp.Rupeezy.RupeezyMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new RupeezyMessageAdapter(Connector.TransactionIdGenerator)
{
	ApplicationId = "<id>",
	ApiKey = "<key>".ToSecureString(),
	AuthCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_rupeezy.md).

## Véase también

[Configuración del conector](configuration_rupeezy.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
