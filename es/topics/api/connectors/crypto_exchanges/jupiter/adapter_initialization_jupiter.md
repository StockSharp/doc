# Inicialización del adaptador Jupiter

El siguiente código muestra cómo inicializar [JupiterMessageAdapter](xref:StockSharp.Jupiter.JupiterMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new JupiterMessageAdapter(Connector.TransactionIdGenerator)
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
