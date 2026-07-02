> [!WARNING]
> Esta bolsa ha cerrado permanentemente (mayo de 2019 — hackeada y liquidada). Este conector ya no está operativo. La documentación se conserva como referencia histórica.

# Inicialización del adaptador Cryptopia

El siguiente código muestra cómo inicializar [CryptopiaMessageAdapter](xref:StockSharp.Cryptopia.CryptopiaMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new CryptopiaMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
