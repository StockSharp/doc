# Inicialización del adaptador: Unusual Whales

El siguiente código inicializa [UnusualWhalesMessageAdapter](xref:StockSharp.UnusualWhales.UnusualWhalesMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new UnusualWhalesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_unusual_whales.md).

## Véase también

[Configuración del conector](configuration_unusual_whales.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
