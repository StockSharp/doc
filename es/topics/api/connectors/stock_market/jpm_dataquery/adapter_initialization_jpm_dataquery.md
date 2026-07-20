# Inicialización del adaptador J.P. Morgan DataQuery

El siguiente código muestra cómo inicializar [JpmDataQueryMessageAdapter](xref:StockSharp.J.P. Morgan DataQuery.JpmDataQueryMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new JpmDataQueryMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Su valor>",
	ClientSecret = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
