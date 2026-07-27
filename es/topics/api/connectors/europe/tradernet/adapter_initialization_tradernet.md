# Inicialización del adaptador: Tradernet

El siguiente código inicializa [TradernetMessageAdapter](xref:StockSharp.Tradernet.TradernetMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradernetMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_tradernet.md).

## Véase también

[Configuración del conector](configuration_tradernet.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
