# Inicialización del adaptador: XBRL Filings

El siguiente código inicializa [XbrlFilingsMessageAdapter](xref:StockSharp.XbrlFilings.XbrlFilingsMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new XbrlFilingsMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_xbrl_filings.md).

## Véase también

[Configuración del conector](configuration_xbrl_filings.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
