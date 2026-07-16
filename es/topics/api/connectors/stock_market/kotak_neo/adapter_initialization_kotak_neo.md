# Inicialización del adaptador Kotak Neo

El código siguiente muestra cómo inicializar [KotakNeoMessageAdapter](xref:StockSharp.KotakNeo.KotakNeoMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KotakNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerKey = "<Clave de consumidor>".ToSecureString(),
	MobileNumber = "<Número de móvil>",
	UserCode = "<Código de usuario>",
	Mpin = "<MPIN>".ToSecureString(),
	TotpSecret = "<Secreto TOTP>".ToSecureString(),
	DefaultProduct = KotakNeoProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

