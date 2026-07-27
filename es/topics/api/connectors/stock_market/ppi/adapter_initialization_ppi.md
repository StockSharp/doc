# Inicialización del adaptador: PPI

El siguiente código inicializa [PpiMessageAdapter](xref:StockSharp.Ppi.PpiMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new PpiMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	AuthorizedClient = "<id>",
	ClientKey = "<key>".ToSecureString(),
	IsDemo = true,
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_ppi.md).

## Véase también

[Configuración del conector](configuration_ppi.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
