> [!CAUTION]
> **La bolsa BitMEX cesó sus operaciones el 23 de septiembre de 2026. El conector ya no funciona; la documentación se conserva únicamente como referencia.**

# Inicialización del adaptador BitMEX

El código siguiente muestra cómo inicializar el [BitmexMessageAdapter](xref:StockSharp.Bitmex.BitmexMessageAdapter) y enviarlo al [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitmexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Su clave API>".To<SecureString>(),
				Secret = "<Su secreto API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
