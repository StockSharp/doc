# Inicialización del adaptador: EXANTE

El siguiente código inicializa [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ExanteMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
	SummaryCurrency = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_exante.md).

## Véase también

[Configuración del conector](configuration_exante.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
