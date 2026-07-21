# Inicialización del adaptador Fireblocks

El siguiente código muestra cómo inicializar [FireblocksMessageAdapter](xref:StockSharp.Fireblocks.FireblocksMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FireblocksMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Su valor>",
	PrivateKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
