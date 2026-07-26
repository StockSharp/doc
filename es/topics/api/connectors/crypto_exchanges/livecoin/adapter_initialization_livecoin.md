> [!CAUTION]
> **La bolsa Livecoin ha cesado sus operaciones. El conector ya no funciona; la documentación se conserva únicamente como referencia.**

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
