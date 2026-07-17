# Inicialización del adaptador NinjaTrader

El código siguiente muestra cómo inicializar [NinjaTraderMessageAdapter](xref:StockSharp.NinjaTrader.NinjaTraderMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new NinjaTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Nombre de usuario>",
	Password = "<Contraseña>".ToSecureString(),
	ClientId = "<Identificador de cliente>",
	Secret = "<Secreto>".ToSecureString(),
	AppId = "StockSharp",
	AppVersion = "1.0",
	DeviceId = Guid.NewGuid().ToString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
