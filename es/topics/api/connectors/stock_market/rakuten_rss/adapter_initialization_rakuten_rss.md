# Inicialización del adaptador Rakuten MARKETSPEED II RSS

El siguiente código muestra cómo inicializar [RakutenRssMessageAdapter](xref:StockSharp.RakutenRss.RakutenRssMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new RakutenRssMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
