> [!WARNING]
> Este exchange cerró permanentemente (julio de 2017 — incautación). Este conector ya no está operativo. La documentación se conserva como referencia histórica.

# Inicialización del adaptador WEX (BTC-e)

El siguiente código muestra cómo inicializar [BtceMessageAdapter](xref:StockSharp.Btce.BtceMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BtceMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
