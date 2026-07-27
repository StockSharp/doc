# Inicialización del adaptador: Nuvama

El siguiente código inicializa [NuvamaMessageAdapter](xref:StockSharp.Nuvama.NuvamaMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new NuvamaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	RequestId = "<id>".ToSecureString(),
	AppIdKey = "<key>".ToSecureString(),
	PublicIpAddress = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_nuvama.md).

## Véase también

[Configuración del conector](configuration_nuvama.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
