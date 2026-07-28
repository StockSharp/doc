# Inicialización del adaptador: Dexalot

El siguiente código inicializa [DexalotMessageAdapter](xref:StockSharp.Dexalot.DexalotMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new DexalotMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<La dirección de su monedero>",
	PrivateKey = "<Su clave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los datos del monedero y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_dexalot.md).

## Véase también

[Configuración del conector](configuration_dexalot.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
