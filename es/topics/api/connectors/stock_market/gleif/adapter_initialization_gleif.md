# Inicialización del adaptador: GLEIF

El siguiente código inicializa [GleifMessageAdapter](xref:StockSharp.Gleif.GleifMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GleifMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_gleif.md).

## Véase también

[Configuración del conector](configuration_gleif.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
