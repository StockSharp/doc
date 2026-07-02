# Inicialización del adaptador RSS

El código a continuación muestra cómo inicializar el [RssMessageAdapter](xref:StockSharp.Rss.RssMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new RssMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = new Uri("http://energy.rss"),
	CustomDateFormat = "ddd, dd MMM yyyy HH:mm:ss zzzz"
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
