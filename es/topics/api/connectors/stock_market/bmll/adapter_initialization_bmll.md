# Inicialización del adaptador BMLL

El siguiente código muestra cómo inicializar [BmllMessageAdapter](xref:StockSharp.Bmll.BmllMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BmllMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Su valor>",
	Password = "<Su valor>".To<SecureString>(),
	Token = "<Su valor>".To<SecureString>(),
	ApiKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
