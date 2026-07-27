# Inicialización del adaptador: DNSE

El siguiente código inicializa [DnseMessageAdapter](xref:StockSharp.Dnse.DnseMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DnseMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	TradingToken = "<token>".ToSecureString(),
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_dnse.md).

## Véase también

[Configuración del conector](configuration_dnse.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
