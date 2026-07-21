# Inicialización del adaptador Reya

El siguiente código muestra cómo inicializar [ReyaMessageAdapter](xref:StockSharp.Reya.ReyaMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ReyaMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Su valor>",
	AccountId = "<Su valor>",
	PrivateKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
