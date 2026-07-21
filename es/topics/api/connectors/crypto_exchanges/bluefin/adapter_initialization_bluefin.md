# Inicialización del adaptador Bluefin

El siguiente código muestra cómo inicializar [BluefinMessageAdapter](xref:StockSharp.Bluefin.BluefinMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BluefinMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Su valor>",
	PrivateKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
