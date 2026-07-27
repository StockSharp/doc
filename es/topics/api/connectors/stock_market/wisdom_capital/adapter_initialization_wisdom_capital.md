# Inicialización del adaptador: Wisdom Capital

El siguiente código inicializa [WisdomCapitalMessageAdapter](xref:StockSharp.WisdomCapital.WisdomCapitalMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new WisdomCapitalMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	MarketDataKey = "<key>".ToSecureString(),
	MarketDataSecret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_wisdom_capital.md).

## Véase también

[Configuración del conector](configuration_wisdom_capital.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
