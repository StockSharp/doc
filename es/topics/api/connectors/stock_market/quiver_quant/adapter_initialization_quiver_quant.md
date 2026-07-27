# Inicialización del adaptador: Quiver Quantitative

El siguiente código inicializa [QuiverQuantMessageAdapter](xref:StockSharp.QuiverQuant.QuiverQuantMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QuiverQuantMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_quiver_quant.md).

## Véase también

[Configuración del conector](configuration_quiver_quant.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
