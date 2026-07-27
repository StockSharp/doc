# Inicialización del adaptador: BCS

El siguiente código inicializa [BcsMessageAdapter](xref:StockSharp.Bcs.BcsMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BcsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_bcs.md).

## Véase también

[Configuración del conector](configuration_bcs.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
