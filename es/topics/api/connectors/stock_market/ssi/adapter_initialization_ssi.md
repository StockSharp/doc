# Inicialización del adaptador: SSI

El siguiente código inicializa [SSIMessageAdapter](xref:StockSharp.SSI.SSIMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SSIMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave de API>".To<SecureString>(),
	Secret = "<Su secreto de API>".To<SecureString>(),
	ClientId = "<Su identificador de cliente>",
	PrivateKey = "<Su clave RSA privada>".To<SecureString>(),
	Otp = "<OTP actual>".To<SecureString>(),
	Account = "<El número de su cuenta>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los valores de acceso y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_ssi.md).

## Véase también

[Configuración del conector](configuration_ssi.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
