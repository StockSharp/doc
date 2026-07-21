# Inicialización del adaptador QFEX

El siguiente código muestra cómo inicializar [QFEXMessageAdapter](xref:StockSharp.QFEX.QFEXMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new QFEXMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Su valor>",
	Secret = "<Su valor>".To<SecureString>(),
	AccountId = "<Su valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
