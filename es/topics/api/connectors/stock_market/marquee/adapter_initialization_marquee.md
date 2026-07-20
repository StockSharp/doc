# Inicialización del adaptador Goldman Sachs Marquee

El siguiente código muestra cómo inicializar [MarqueeMessageAdapter](xref:StockSharp.Marquee.MarqueeMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MarqueeMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Su valor>",
	ClientSecret = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
