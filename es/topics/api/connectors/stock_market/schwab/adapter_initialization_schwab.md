# Inicialización del adaptador Charles Schwab

El siguiente código muestra cómo inicializar [SchwabMessageAdapter](xref:StockSharp.Schwab.SchwabMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SchwabMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token de acceso>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
