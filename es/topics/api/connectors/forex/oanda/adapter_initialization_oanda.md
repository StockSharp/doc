# Inicialización del adaptador Oanda

El siguiente código muestra cómo inicializar [OandaMessageAdapter](xref:StockSharp.Oanda.OandaMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new OandaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Su token>".To<SecureString>(),
	IsDemo = true, // Demo
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
