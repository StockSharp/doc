# Inicialización del adaptador: Ventura

El siguiente código inicializa [VenturaMessageAdapter](xref:StockSharp.Ventura.VenturaMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new VenturaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	ClientId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_ventura.md).

## Véase también

[Configuración del conector](configuration_ventura.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
