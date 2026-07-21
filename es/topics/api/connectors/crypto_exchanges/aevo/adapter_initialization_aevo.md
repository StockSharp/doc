# Inicialización del adaptador Aevo

El siguiente código muestra cómo inicializar [AevoMessageAdapter](xref:StockSharp.Aevo.AevoMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AevoMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Su valor>",
	ApiSecret = "<Su valor>".To<SecureString>(),
	WalletAddress = "<Su valor>",
	SigningKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
