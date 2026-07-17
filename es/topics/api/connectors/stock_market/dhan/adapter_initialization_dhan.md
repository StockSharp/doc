# Inicialización del adaptador DhanHQ

El código siguiente muestra cómo inicializar [DhanMessageAdapter](xref:StockSharp.Dhan.DhanMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DhanMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Identificador de cliente>",
	Token = "<Token>".ToSecureString(),
	DefaultProduct = DhanProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
