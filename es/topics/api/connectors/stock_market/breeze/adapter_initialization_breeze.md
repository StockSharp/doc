# Inicialización del adaptador ICICI Direct Breeze

El código siguiente muestra cómo inicializar [BreezeMessageAdapter](xref:StockSharp.Breeze.BreezeMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BreezeMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Clave de API>",
	SecretKey = "<Clave secreta>".ToSecureString(),
	ApiSession = "<Sesión de API>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

