# Inicialización del adaptador Polymarket

El siguiente código muestra cómo inicializar [PolymarketMessageAdapter](xref:StockSharp.Polymarket.PolymarketMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PolymarketMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Su valor>",
	ApiSecret = "<Su valor>".To<SecureString>(),
	Passphrase = "<Su valor>".To<SecureString>(),
	SignerAddress = "<Su valor>",
	FunderAddress = "<Su valor>",
	PrivateKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
