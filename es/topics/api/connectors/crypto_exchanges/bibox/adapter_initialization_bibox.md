# Inicialización del adaptador Bibox

El código a continuación muestra cómo inicializar [BiboxMessageAdapter](xref:StockSharp.Bibox.BiboxMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BiboxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
