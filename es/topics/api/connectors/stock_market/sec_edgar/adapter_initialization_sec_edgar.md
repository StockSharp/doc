# Inicialización del adaptador: SEC EDGAR

El siguiente código inicializa [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SecEdgarMessageAdapter(connector.TransactionIdGenerator)
{
	UserAgent = "<Su aplicación su-correo@example.com>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los valores de acceso y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_sec_edgar.md).

## Véase también

[Configuración del conector](configuration_sec_edgar.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
