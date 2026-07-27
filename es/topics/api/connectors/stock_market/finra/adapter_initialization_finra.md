# Inicialización del adaptador: FINRA

El siguiente código inicializa [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinraMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_finra.md).

## Véase también

[Configuración del conector](configuration_finra.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
