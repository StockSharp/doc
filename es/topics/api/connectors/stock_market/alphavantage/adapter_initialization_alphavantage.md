# Inicialización del adaptador AlphaVantage

El siguiente código muestra cómo inicializar [AlphaVantageMessageAdapter](xref:StockSharp.AlphaVantage.AlphaVantageMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new AlphaVantageMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Su token>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
			
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
