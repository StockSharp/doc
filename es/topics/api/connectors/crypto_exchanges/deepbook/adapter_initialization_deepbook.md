# Inicialización del adaptador: DeepBook

El siguiente código inicializa [DeepBookMessageAdapter](xref:StockSharp.DeepBook.DeepBookMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new DeepBookMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<La dirección de su monedero>",
	PrivateKey = "<Su clave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_deepbook.md).

## Véase también

[Configuración del conector](configuration_deepbook.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
