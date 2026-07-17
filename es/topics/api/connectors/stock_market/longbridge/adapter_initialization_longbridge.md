# Inicialización del adaptador Longbridge OpenAPI

El código siguiente muestra cómo inicializar [LongbridgeMessageAdapter](xref:StockSharp.Longbridge.LongbridgeMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LongbridgeMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<Clave de aplicación>",
	AppSecret = "<Secreto de aplicación>".ToSecureString(),
	AccessToken = "<Token de acceso>".ToSecureString(),
	Portfolio = "Longbridge",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
