# Inicialización del adaptador: SET Market Data

El siguiente código inicializa [SetMarketDataMessageAdapter](xref:StockSharp.SetMarketData.SetMarketDataMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SetMarketDataMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_set_market_data.md).

## Véase también

[Configuración del conector](configuration_set_market_data.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
