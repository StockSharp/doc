# Inicialización del adaptador: XRPL DEX

El siguiente código inicializa [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new XrplMessageAdapter(connector.TransactionIdGenerator)
{
	Account = "<La dirección de su cuenta XRPL>",
	Seed = "<Su semilla familiar secreta>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los datos de la cuenta y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_xrpl.md).

## Véase también

[Configuración del conector](configuration_xrpl.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
