# Inicialización del adaptador Bitalong

El siguiente código muestra cómo inicializar [BitalongMessageAdapter](xref:StockSharp.Bitalong.BitalongMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitalongMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
