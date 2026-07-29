# Inicialización del adaptador: MarketData.app

El siguiente código inicializa [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new MarketDataAppMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Su token de API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los valores de acceso y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_marketdataapp.md).

## Véase también

[Configuración del conector](configuration_marketdataapp.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
