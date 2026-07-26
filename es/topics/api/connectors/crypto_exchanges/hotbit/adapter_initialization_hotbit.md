> [!CAUTION]
> **La bolsa Hotbit ha cesado sus operaciones. El conector ya no funciona; la documentación se conserva únicamente como referencia.**

# Inicialización del adaptador Hotbit

El siguiente código muestra cómo inicializar [HotbitMessageAdapter](xref:StockSharp.Hotbit.HotbitMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new HotbitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Su clave API>".To<SecureString>(),
				Secret = "<Su secreto API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
