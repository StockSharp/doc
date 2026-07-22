# Inicialización del adaptador Tardis

El siguiente código muestra cómo inicializar [TardisMessageAdapter](xref:StockSharp.Tardis.TardisMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TardisMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Su valor>".To<SecureString>(),
	Exchange = "<Su valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
