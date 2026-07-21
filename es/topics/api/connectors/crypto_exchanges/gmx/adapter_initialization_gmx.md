# Inicialización del adaptador GMX

El siguiente código muestra cómo inicializar [GmxMessageAdapter](xref:StockSharp.Gmx.GmxMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GmxMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Su valor>",
	PrivateKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
