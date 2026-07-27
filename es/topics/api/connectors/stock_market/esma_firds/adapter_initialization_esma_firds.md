# Inicialización del adaptador: ESMA FIRDS

El siguiente código inicializa [EsmaFirdsMessageAdapter](xref:StockSharp.EsmaFirds.EsmaFirdsMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EsmaFirdsMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_esma_firds.md).

## Véase también

[Configuración del conector](configuration_esma_firds.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
