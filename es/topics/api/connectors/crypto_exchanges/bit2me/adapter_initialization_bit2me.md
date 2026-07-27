# Inicialización del adaptador Bit2Me

El siguiente código muestra cómo inicializar [Bit2MeMessageAdapter](xref:StockSharp.Bit2Me.Bit2MeMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new Bit2MeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave API>".To<SecureString>(),
	Secret = "<Su secreto API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Omita `Key` y `Secret` si solo necesita datos públicos. Las direcciones REST y WebSocket pueden cambiarse mediante `RestEndpoint` y `WebSocketEndpoint`.

## Contenido recomendado

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
