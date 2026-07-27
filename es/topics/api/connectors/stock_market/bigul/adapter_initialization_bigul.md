# Inicialización del adaptador: Bigul

El siguiente código inicializa [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BigulMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientCode = "<id>",
	ApiKey = "<key>".ToSecureString(),
	ApiSecret = "<secret>".ToSecureString(),
	OneTimePassword = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	Source = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_bigul.md).

## Véase también

[Configuración del conector](configuration_bigul.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
