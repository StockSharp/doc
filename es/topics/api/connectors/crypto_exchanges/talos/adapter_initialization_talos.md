# Inicialización del adaptador Talos

El siguiente código muestra cómo inicializar [TalosMessageAdapter](xref:StockSharp.Talos.TalosMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TalosMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<Su valor>".To<EndPoint>(),
	SenderCompId = "<Su valor>",
	TargetCompId = "<Su valor>",
	Login = "<Su valor>",
	Password = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
