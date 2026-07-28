# Inicialización del adaptador: DEX Screener

El siguiente código inicializa [DexScreenerMessageAdapter](xref:StockSharp.DexScreener.DexScreenerMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new DexScreenerMessageAdapter(connector.TransactionIdGenerator);
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_dex_screener.md).

## Véase también

[Configuración del conector](configuration_dex_screener.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
