# Inicialización del adaptador Questrade

El código siguiente muestra cómo inicializar [QuestradeMessageAdapter](xref:StockSharp.Questrade.QuestradeMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QuestradeMessageAdapter(Connector.TransactionIdGenerator)
{
	RefreshToken = "<Token de actualización>".ToSecureString(),
	Account = "<Cuenta>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

