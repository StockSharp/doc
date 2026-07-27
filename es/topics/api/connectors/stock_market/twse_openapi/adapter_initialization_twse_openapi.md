# Inicialización del adaptador: TWSE

El siguiente código inicializa [TwseMessageAdapter](xref:StockSharp.Twse.TwseMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TwseMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_twse_openapi.md).

## Véase también

[Configuración del conector](configuration_twse_openapi.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
