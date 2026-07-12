# Inicialización del adaptador CoinHub

El siguiente código muestra cómo inicializar [CoinHubMessageAdapter](xref:StockSharp.CoinHub.CoinHubMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new CoinHubMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Su clave API>".To<SecureString>(),
				Secret = "<Su secreto API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
