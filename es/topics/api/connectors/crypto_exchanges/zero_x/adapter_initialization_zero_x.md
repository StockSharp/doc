# Inicialización del adaptador: 0x

El siguiente código inicializa [ZeroXMessageAdapter](xref:StockSharp.ZeroX.ZeroXMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ZeroXMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<Su clave API>".To<SecureString>(),
	WalletAddress = "<La dirección de su monedero>",
	PrivateKey = "<Su clave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_zero_x.md).

## Véase también

[Configuración del conector](configuration_zero_x.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
