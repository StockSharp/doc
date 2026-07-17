# Inicialización del adaptador: Kiwoom

El código siguiente muestra cómo inicializar [KiwoomMessageAdapter](xref:StockSharp.Kiwoom.KiwoomMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KiwoomMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<valor>".ToSecureString(),
	AppSecret = "<valor>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
