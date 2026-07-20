# Inicialización del adaptador GRVT

El siguiente código muestra cómo inicializar [GrvtMessageAdapter](xref:StockSharp.Grvt.GrvtMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GrvtMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Su valor>".To<SecureString>(),
	Secret = "<Su valor>".To<SecureString>(),
	SubAccountId = "<Su valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
