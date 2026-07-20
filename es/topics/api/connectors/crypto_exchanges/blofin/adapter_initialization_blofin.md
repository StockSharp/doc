# Inicialización del adaptador BloFin

El siguiente código muestra cómo inicializar [BloFinMessageAdapter](xref:StockSharp.BloFin.BloFinMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BloFinMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Su valor>".To<SecureString>(),
	Secret = "<Su valor>".To<SecureString>(),
	Passphrase = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
