> [!WARNING]
> esta bolsa cerró permanentemente (~2023 — efectivamente inactivo). Este conector ya no está operativo. La documentación se conserva como referencia histórica.

# Inicialización del adaptador Yobit

El siguiente código muestra cómo inicializar [YobitMessageAdapter](xref:StockSharp.Yobit.YobitMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new YobitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Su clave API>".To<SecureString>(),
				Secret = "<Su secreto API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
