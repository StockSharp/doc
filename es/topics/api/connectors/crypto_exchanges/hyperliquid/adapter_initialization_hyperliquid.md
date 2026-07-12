# Inicialización del adaptador Hyperliquid

El siguiente código muestra cómo inicializar [HyperliquidMessageAdapter](xref:StockSharp.Hyperliquid.HyperliquidMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new HyperliquidMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Su dirección de monedero>",
	PrivateKey = "<Su clave privada>".To<SecureString>(),
	Section = HyperliquidSections.Derivatives,
	IsTestnet = false,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
