# Inicialización del adaptador: GuruFocus

El siguiente código inicializa [GuruFocusMessageAdapter](xref:StockSharp.GuruFocus.GuruFocusMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GuruFocusMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_gurufocus.md).

## Véase también

[Configuración del conector](configuration_gurufocus.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
