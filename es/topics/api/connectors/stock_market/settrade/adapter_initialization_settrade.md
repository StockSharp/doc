# Inicialización del adaptador: Settrade

El siguiente código inicializa [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SettradeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave de API>".To<SecureString>(),
	Secret = "<Su secreto de API>".To<SecureString>(),
	AppCode = "<El código de su aplicación>",
	BrokerId = "<El identificador de su corredor>",
	Account = "<El número de su cuenta>",
	Pin = "<Su PIN de negociación>".To<SecureString>(),
	AccountType = SettradeAccountTypes.Equity,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales, el tipo de cuenta y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_settrade.md).

## Véase también

[Configuración del conector](configuration_settrade.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
