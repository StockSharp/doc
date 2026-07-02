> [!WARNING]
> Esta bolsa ha cerrado permanentemente (octubre de 2019 — cerrada). Este conector ya no está operativo. La documentación se conserva como referencia histórica.

# Inicialización del adaptador CoinExchange

El siguiente código muestra cómo inicializar [CoinExchangeMessageAdapter](xref:StockSharp.CoinExchange.CoinExchangeMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new  CoinExchangeMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
