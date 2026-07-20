# Inicialización del adaptador Nasdaq Data Link

El siguiente código muestra cómo inicializar [NasdaqDataLinkMessageAdapter](xref:StockSharp.NasdaqDataLink.NasdaqDataLinkMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new NasdaqDataLinkMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
