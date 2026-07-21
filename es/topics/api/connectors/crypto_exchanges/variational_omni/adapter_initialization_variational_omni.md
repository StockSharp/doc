# Inicialización del adaptador Variational Omni

El siguiente código muestra cómo inicializar [VariationalOmniMessageAdapter](xref:StockSharp.VariationalOmni.VariationalOmniMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new VariationalOmniMessageAdapter(Connector.TransactionIdGenerator)
{
	Endpoint = "<Su valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
