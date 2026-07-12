> [!WARNING]
> Este exchange cerró permanentemente (diciembre de 2020 — hackeo y cierre). Este conector ya no está operativo. La documentación se conserva como referencia histórica.

# Inicialización del adaptador Livecoin

El siguiente código muestra cómo inicializar [LiveCoinMessageAdapter](xref:StockSharp.LiveCoin.LiveCoinMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new LiveCoinMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Su clave API>".To<SecureString>(),
				Secret = "<Su secreto API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
