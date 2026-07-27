# Inicialización del adaptador: TPEx

El siguiente código inicializa [TpexMessageAdapter](xref:StockSharp.Tpex.TpexMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TpexMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_tpex.md).

## Véase también

[Configuración del conector](configuration_tpex.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
