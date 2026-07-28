# Inicialización del adaptador: Velora

El siguiente código inicializa [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new VeloraMessageAdapter(connector.TransactionIdGenerator)
{
	Partner = "<Su identificador de socio>",
	WalletAddress = "<La dirección de su monedero>",
	PrivateKey = "<Su clave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_velora.md).

## Véase también

[Configuración del conector](configuration_velora.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
