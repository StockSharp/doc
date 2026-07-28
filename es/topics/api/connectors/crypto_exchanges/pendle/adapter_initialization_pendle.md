# Inicialización del adaptador: Pendle

El siguiente código inicializa [PendleMessageAdapter](xref:StockSharp.Pendle.PendleMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new PendleMessageAdapter(connector.TransactionIdGenerator)
{
	Chain = PendleChains.Ethereum,
	WalletAddress = "<La dirección de su monedero>",
	PrivateKey = "<Su clave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los datos del monedero y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_pendle.md).

## Véase también

[Configuración del conector](configuration_pendle.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
