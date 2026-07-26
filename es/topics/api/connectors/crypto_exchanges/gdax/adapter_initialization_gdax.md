> [!CAUTION]
> **El servicio GDAX ya no está disponible. Su sucesor, Coinbase Pro, también fue retirado; para Coinbase debe utilizarse el conector Coinbase actual. Este conector no funciona; la documentación se conserva únicamente como referencia.**

# Inicialización del adaptador GDAX

El siguiente código muestra cómo inicializar [GdaxMessageAdapter](xref:StockSharp.Gdax.GdaxMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new GdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Su clave API>".To<SecureString>(),
				Secret = "<Su secreto API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
