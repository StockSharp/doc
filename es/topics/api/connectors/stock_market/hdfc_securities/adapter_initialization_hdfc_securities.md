# Inicialización del adaptador: HDFC Securities

El siguiente código inicializa [HdfcMessageAdapter](xref:StockSharp.HdfcSecurities.HdfcMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new HdfcMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	RequestToken = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_hdfc_securities.md).

## Véase también

[Configuración del conector](configuration_hdfc_securities.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
