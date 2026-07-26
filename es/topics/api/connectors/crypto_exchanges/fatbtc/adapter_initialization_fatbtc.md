> [!CAUTION]
> **La bolsa FatBTC ha cesado sus operaciones. El conector ya no funciona; la documentación se conserva únicamente como referencia.**

# Inicialización del adaptador FatBTC

El siguiente código muestra cómo inicializar [FatBtcMessageAdapter](xref:StockSharp.FatBTC.FatBtcMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new FatBtcMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Su clave API>".To<SecureString>(),
				Secret = "<Su secreto API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
