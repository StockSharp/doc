# Inicialización del adaptador: InvertirOnline

El siguiente código inicializa [InvertirOnlineMessageAdapter](xref:StockSharp.InvertirOnline.InvertirOnlineMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new InvertirOnlineMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_invertironline.md).

## Véase también

[Configuración del conector](configuration_invertironline.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
