# Inicialización del adaptador: Primary

El siguiente código inicializa [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new PrimaryMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	IsDemo = true,
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_primary.md).

## Véase también

[Configuración del conector](configuration_primary.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
