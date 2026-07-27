# Inicialización del adaptador: MasterLink

El siguiente código inicializa [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MasterLinkMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	CertificatePath = "<id>",
	CertificatePassword = "<secret>".ToSecureString(),
	NodePath = "<id>",
	GatewayDirectory = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_masterlink.md).

## Véase también

[Configuración del conector](configuration_masterlink.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
