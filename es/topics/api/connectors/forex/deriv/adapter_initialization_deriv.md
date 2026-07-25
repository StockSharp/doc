# Inicialización del adaptador: Deriv

El siguiente código inicializa [DerivMessageAdapter](xref:StockSharp.Deriv.DerivMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DerivMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AppId = "<app-id>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por el token y el identificador de aplicación emitidos para la cuenta demo o real seleccionada.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
