# Inicialización del adaptador Dow Jones

El siguiente código muestra cómo inicializar [DowJonesMessageAdapter](xref:StockSharp.DowJones.DowJonesMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new DowJonesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Su valor>".To<SecureString>(),
	ClientId = "<Su valor>",
	Login = "<Su valor>",
	Password = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
