# Inicialización del adaptador: Firstock

El siguiente código inicializa [FirstockMessageAdapter](xref:StockSharp.Firstock.FirstockMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FirstockMessageAdapter(Connector.TransactionIdGenerator)
{
	UserId = "<id>",
	Password = "<secret>".ToSecureString(),
	OneTimePassword = "<secret>".ToSecureString(),
	VendorCode = "<id>",
	ApiKey = "<key>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_firstock.md).

## Véase también

[Configuración del conector](configuration_firstock.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
