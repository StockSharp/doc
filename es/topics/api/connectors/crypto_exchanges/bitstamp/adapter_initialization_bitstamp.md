# Inicialización del adaptador BitStamp

El código siguiente muestra cómo inicializar el [BitStampMessageAdapter](xref:StockSharp.BitStamp.BitStampMessageAdapter) y enviarlo al [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitStampMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
