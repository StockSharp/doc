# Inicialización del adaptador: KyberSwap

El siguiente código inicializa [KyberSwapMessageAdapter](xref:StockSharp.KyberSwap.KyberSwapMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new KyberSwapMessageAdapter(connector.TransactionIdGenerator)
{
	ClientId = "<Su identificador de cliente>",
	WalletAddress = "<La dirección de su monedero>",
	PrivateKey = "<Su clave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_kyber_swap.md).

## Véase también

[Configuración del conector](configuration_kyber_swap.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
