# Inicialización del adaptador: Marketaux

El siguiente código inicializa [MarketauxMessageAdapter](xref:StockSharp.Marketaux.MarketauxMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MarketauxMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_marketaux.md).

## Véase también

[Configuración del conector](configuration_marketaux.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
