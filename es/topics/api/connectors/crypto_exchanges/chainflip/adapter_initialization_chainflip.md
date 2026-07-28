# Inicialización del adaptador: Chainflip

El siguiente código inicializa [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ChainflipMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<La dirección de su monedero EVM>",
	PrivateKey = "<Su clave privada EVM>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los datos del monedero, las direcciones de destino y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_chainflip.md).

## Véase también

[Configuración del conector](configuration_chainflip.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
