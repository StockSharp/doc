# Inicialización del adaptador Kalshi

El siguiente código muestra cómo inicializar [KalshiMessageAdapter](xref:StockSharp.Kalshi.KalshiMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new KalshiMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Su valor>",
	PrivateKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
