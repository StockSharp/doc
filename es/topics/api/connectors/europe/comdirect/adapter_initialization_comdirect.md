# Inicialización del adaptador: comdirect

El siguiente código inicializa [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ComdirectMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_comdirect.md).

## Véase también

[Configuración del conector](configuration_comdirect.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
