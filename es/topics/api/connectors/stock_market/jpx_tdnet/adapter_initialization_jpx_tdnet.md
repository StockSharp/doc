# Inicialización del adaptador: JPX TDnet

El siguiente código inicializa [JpxTdnetMessageAdapter](xref:StockSharp.JpxTdnet.JpxTdnetMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new JpxTdnetMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_jpx_tdnet.md).

## Véase también

[Configuración del conector](configuration_jpx_tdnet.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
