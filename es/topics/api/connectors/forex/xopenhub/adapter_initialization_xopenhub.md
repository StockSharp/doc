# Inicialización del adaptador X Open Hub

El siguiente código muestra cómo inicializar [XOpenHubMessageAdapter](xref:StockSharp.XOpenHub.XOpenHubMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new XOpenHubMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Su valor>",
	Password = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
