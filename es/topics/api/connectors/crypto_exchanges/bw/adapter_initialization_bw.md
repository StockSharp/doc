> [!WARNING]
> Este exchange ha cerrado permanentemente (~2021 — cierre). Este conector ya no está operativo. La documentación se conserva como referencia histórica.

# Inicialización del adaptador BW

El código siguiente muestra cómo inicializar el [BWMessageAdapter](xref:StockSharp.BW.BWMessageAdapter) y enviarlo al [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BWMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Su clave API>".To<SecureString>(),
				Secret = "<Su secreto API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
