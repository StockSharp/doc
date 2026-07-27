# Inicialización del adaptador: TASE Data Hub

El siguiente código inicializa [TaseDataHubMessageAdapter](xref:StockSharp.TaseDataHub.TaseDataHubMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TaseDataHubMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_tase_data_hub.md).

## Véase también

[Configuración del conector](configuration_tase_data_hub.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
