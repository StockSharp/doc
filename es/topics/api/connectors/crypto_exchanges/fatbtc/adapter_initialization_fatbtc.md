> [!WARNING]
> Esta bolsa ha cerrado permanentemente (~2022 — cierre). Este conector ya no está operativo. La documentación se conserva como referencia histórica.

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
