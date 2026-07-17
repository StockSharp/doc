# Inicialización del adaptador: 5paisa Xstream

El código siguiente muestra cómo inicializar [FivePaisaMessageAdapter](xref:StockSharp.FivePaisa.FivePaisaMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FivePaisaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<valor>".ToSecureString(),
	AppKey = "<valor>",
	ClientCode = "<valor>",
	AlgoId = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
