# Inicialización del adaptador BitGo

El siguiente código muestra cómo inicializar [BitGoMessageAdapter](xref:StockSharp.BitGo.BitGoMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BitGoMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Su valor>".To<SecureString>(),
	Account = "<Su valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
