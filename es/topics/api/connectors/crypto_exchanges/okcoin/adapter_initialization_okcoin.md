> [!CAUTION]
> **Los servicios de negociación de OKCoin se desactivaron tras la transición de la plataforma a OKX. Este conector ya no funciona; la documentación se conserva únicamente como referencia.**

# Inicialización del adaptador OKCoin

El siguiente código muestra cómo inicializar [OkcoinMessageAdapter](xref:StockSharp.Okcoin.OkcoinMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new OkcoinMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Su clave API>".To<SecureString>(),
				Secret = "<Su secreto API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
