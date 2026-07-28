# Inicialización del adaptador: Tokocrypto

El siguiente código inicializa [TokocryptoMessageAdapter](xref:StockSharp.Tokocrypto.TokocryptoMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new TokocryptoMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave API>".To<SecureString>(),
	Secret = "<Su secreto API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_tokocrypto.md).

## Véase también

[Configuración del conector](configuration_tokocrypto.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
