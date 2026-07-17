# Inicialización del adaptador: Sierra Chart DTC

El código siguiente muestra cómo inicializar [SierraChartDtcMessageAdapter](xref:StockSharp.SierraChartDtc.SierraChartDtcMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SierraChartDtcMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	Login = "<valor>",
	TradeAccount = "<valor>",
	TargetHost = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
