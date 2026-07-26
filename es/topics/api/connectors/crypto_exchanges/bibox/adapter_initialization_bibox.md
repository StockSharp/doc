> [!CAUTION]
> **La API de la bolsa Bibox que utiliza este conector ya no está disponible. El conector no funciona; la documentación se conserva únicamente como referencia.**

# Inicialización del adaptador Bibox

El código a continuación muestra cómo inicializar [BiboxMessageAdapter](xref:StockSharp.Bibox.BiboxMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BiboxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Su clave API>".To<SecureString>(),
				Secret = "<Su secreto API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
