# Inicialización del adaptador Moomoo

El código siguiente muestra cómo inicializar [MoomooMessageAdapter](xref:StockSharp.Moomoo.MoomooMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MoomooMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Loopback, 11111),
	Password = "<Contraseña>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
