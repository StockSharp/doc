# Inicialización del adaptador Phillip POEMS

El siguiente código muestra cómo inicializar [PhillipPoemsMessageAdapter](xref:StockSharp.PhillipPoems.PhillipPoemsMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PhillipPoemsMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Su valor>",
	ClientSecret = "<Su valor>".To<SecureString>(),
	ApiKey = "<Su valor>".To<SecureString>(),
	AccessToken = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
