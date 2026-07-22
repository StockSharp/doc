# Inicialización del adaptador Zero Hash

El siguiente código muestra cómo inicializar [ZeroHashMessageAdapter](xref:StockSharp.ZeroHash.ZeroHashMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ZeroHashMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Su valor>",
	Secret = "<Su valor>".To<SecureString>(),
	Passphrase = "<Su valor>".To<SecureString>(),
	Account = "<Su valor>",
	User = "<Su valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
