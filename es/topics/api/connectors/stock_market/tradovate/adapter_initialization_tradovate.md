# Inicialización del adaptador Tradovate

El código siguiente muestra cómo inicializar [TradovateMessageAdapter](xref:StockSharp.Tradovate.TradovateMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradovateMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<nombre de usuario>",
	Password = "<contraseña>".ToSecureString(),
	ClientId = "<identificador del cliente de API>",
	Secret = "<secreto del cliente de API>".ToSecureString(),
	AppId = "StockSharp",
	AppVersion = "1.0",
	DeviceId = "<identificador estable del dispositivo>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Establezca `IsDemo` en `false` para conectarse al entorno real.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
