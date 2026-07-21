# Inicialización del adaptador FalconX

El siguiente código muestra cómo inicializar [FalconXMessageAdapter](xref:StockSharp.FalconX.FalconXMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FalconXMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Su valor>",
	Secret = "<Su valor>".To<SecureString>(),
	Passphrase = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
