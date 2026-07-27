# Inicialización del adaptador: StockData.org

El siguiente código inicializa [StockDataOrgMessageAdapter](xref:StockSharp.StockDataOrg.StockDataOrgMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new StockDataOrgMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_stockdata_org.md).

## Véase también

[Configuración del conector](configuration_stockdata_org.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
