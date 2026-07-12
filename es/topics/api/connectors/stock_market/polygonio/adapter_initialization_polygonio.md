# Inicialización del adaptador PolygonIO

El siguiente código muestra cómo inicializar [PolygonIOMessageAdapter](xref:StockSharp.PolygonIO.PolygonIOMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new PolygonIOMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Su token>".To<SecureString>(),
	ConnectionType = PolygonIOConnectionTypes.History, // conexión para fuentes de datos REST
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
			
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
