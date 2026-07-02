> [!WARNING]
> Este exchange ha cerrado permanentemente (~2021 — cierre). Este conector ya no está operativo. La documentación se conserva como referencia histórica.

# Inicialización del adaptador CoinBene

El código siguiente muestra cómo inicializar [CoinBeneMessageAdapter](xref:StockSharp.CoinBene.CoinBeneMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new CoinBeneMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
