# Inicialización del adaptador: CoinCatch

El siguiente código inicializa [CoinCatchMessageAdapter](xref:StockSharp.CoinCatch.CoinCatchMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinCatchMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave API>".To<SecureString>(),
	Secret = "<Su secreto API>".To<SecureString>(),
	Passphrase = "<Su frase de acceso API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_coincatch.md).

## Véase también

[Configuración del conector](configuration_coincatch.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
