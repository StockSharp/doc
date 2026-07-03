# Inicialización del adaptador IQFeed

El siguiente código muestra cómo inicializar [IQFeedMessageAdapter](xref:StockSharp.IQFeed.IQFeedMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new IQFeedMessageAdapter(Connector.TransactionIdGenerator)
{
	Level1Address = "127.0.0.1:5009".To<EndPoint>(),
	Level2Address = "127.0.0.1:9200".To<EndPoint>(),
	LookupAddress = "127.0.0.1:9100".To<EndPoint>(),
	AdminAddress =  "127.0.0.1:9200".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
