# Inicialización del adaptador Orderly Network

El siguiente código muestra cómo inicializar [OrderlyNetworkMessageAdapter](xref:StockSharp.OrderlyNetwork.OrderlyNetworkMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OrderlyNetworkMessageAdapter(Connector.TransactionIdGenerator)
{
	AccountId = "<Su valor>",
	Secret = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
