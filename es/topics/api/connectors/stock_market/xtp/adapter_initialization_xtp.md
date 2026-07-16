# Inicialización del adaptador Zhongtai XTP

El código siguiente muestra cómo inicializar [XtpMessageAdapter](xref:StockSharp.Xtp.XtpMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new XtpMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Nombre de usuario>",
	Password = "<Contraseña>".ToSecureString(),
	ClientId = 1,
	QuoteAddress = new IPEndPoint(IPAddress.Parse("203.0.113.10"), 6001),
	TransactionAddress = new IPEndPoint(IPAddress.Parse("203.0.113.10"), 6002),
	Protocol = XtpProtocols.Tcp,
	SoftwareKey = "<Clave del software>",
	SoftwareVersion = "1.0",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

