# Inicialización del adaptador: m.Stock

El siguiente código inicializa [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new MStockMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave de API>".To<SecureString>(),
	ClientCode = "<Su código de cliente>",
	Password = "<Su contraseña>".To<SecureString>(),
	Otp = "<OTP actual>".To<SecureString>(),
	UseTotp = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los valores de acceso y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_mstock.md).

## Véase también

[Configuración del conector](configuration_mstock.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
