> [!CAUTION]
> **La bolsa Bitalong y su API ya no están disponibles. El conector no funciona; la documentación se conserva únicamente como referencia.**

# Inicialización del adaptador Bitalong

El siguiente código muestra cómo inicializar [BitalongMessageAdapter](xref:StockSharp.Bitalong.BitalongMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitalongMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Su clave API>".To<SecureString>(),
				Secret = "<Su secreto API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
