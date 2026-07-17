# Inicialización del adaptador: LS Securities

El código siguiente muestra cómo inicializar [LsSecuritiesMessageAdapter](xref:StockSharp.LsSecurities.LsSecuritiesMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LsSecuritiesMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<valor>".ToSecureString(),
	AppSecret = "<valor>".ToSecureString(),
	Account = "<valor>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
