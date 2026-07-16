# Inicialización del adaptador Angel One

El código siguiente muestra cómo inicializar [AngelOneMessageAdapter](xref:StockSharp.AngelOne.AngelOneMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new AngelOneMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Nombre de usuario>",
	Password = "<Contraseña>".ToSecureString(),
	ApiKey = "<Clave de API>".ToSecureString(),
	TotpSecret = "<Secreto TOTP>".ToSecureString(),
	ClientLocalIp = "127.0.0.1",
	ClientPublicIp = "<Dirección IP pública del cliente>",
	MacAddress = "<Dirección MAC>",
	DefaultProduct = AngelOneProducts.Delivery,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

