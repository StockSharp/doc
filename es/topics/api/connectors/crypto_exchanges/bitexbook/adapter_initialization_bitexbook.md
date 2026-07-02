# Inicialización del adaptador Bitexbook

El siguiente código muestra cómo inicializar [BitexbookMessageAdapter](xref:StockSharp.Bitexbook.BitexbookMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitexbookMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
