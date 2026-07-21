# Inicialización del adaptador Extended

El siguiente código muestra cómo inicializar [ExtendedMessageAdapter](xref:StockSharp.Extended.ExtendedMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ExtendedMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Su valor>".To<SecureString>(),
	PrivateKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
