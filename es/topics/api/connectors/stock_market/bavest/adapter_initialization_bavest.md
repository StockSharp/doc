# Inicialización del adaptador: Bavest

El siguiente código inicializa [BavestMessageAdapter](xref:StockSharp.Bavest.BavestMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BavestMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_bavest.md).

## Véase también

[Configuración del conector](configuration_bavest.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
