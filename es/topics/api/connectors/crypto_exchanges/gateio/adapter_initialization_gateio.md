# Inicialización del adaptador Gate.io

El siguiente código muestra cómo inicializar [GateIOMessageAdapter](xref:StockSharp.GateIO.GateIOMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GateIOMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Su clave API>".To<SecureString>(),
	Secret = "<Su secreto API>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
