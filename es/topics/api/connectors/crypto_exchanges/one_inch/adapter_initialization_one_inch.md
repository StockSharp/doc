# Inicialización del adaptador 1inch

El siguiente código muestra cómo inicializar [OneInchMessageAdapter](xref:StockSharp.OneInch.OneInchMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OneInchMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Su valor>".To<SecureString>(),
	WalletAddress = "<Su valor>",
	PrivateKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
