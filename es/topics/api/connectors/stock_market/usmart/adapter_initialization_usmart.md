# Inicialización del adaptador uSMART OpenAPI

El siguiente código muestra cómo inicializar [UsmartMessageAdapter](xref:StockSharp.Usmart.UsmartMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new UsmartMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<Su valor>".To<SecureString>(),
	PrivateKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
