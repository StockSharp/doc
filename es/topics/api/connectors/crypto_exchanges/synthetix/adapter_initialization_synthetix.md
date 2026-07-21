# Inicialización del adaptador Synthetix

El siguiente código muestra cómo inicializar [SynthetixMessageAdapter](xref:StockSharp.Synthetix.SynthetixMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new SynthetixMessageAdapter(Connector.TransactionIdGenerator)
{
	SubAccountId = "<Su valor>",
	PrivateKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
