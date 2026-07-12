# Inicialización del adaptador Alpaca

El siguiente código muestra cómo inicializar [AlpacaMessageAdapter](xref:StockSharp.Alpaca.AlpacaMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new AlpacaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Su clave API>".To<SecureString>(),
	Secret = "<Su secreto API>".To<SecureString>(),

	// descomentar para modo sandbox
	//IsDemo = true,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
