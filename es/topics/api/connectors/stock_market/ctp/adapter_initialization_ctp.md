# Inicialización del adaptador CTP

El código siguiente muestra cómo inicializar [CtpMessageAdapter](xref:StockSharp.Ctp.CtpMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CtpMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Nombre de usuario>",
	Password = "<Contraseña>".ToSecureString(),
	BrokerId = "<Identificador del bróker>",
	InvestorId = "<Identificador del inversor>",
	MarketDataAddress = "tcp://<Dirección de datos de mercado>",
	TraderAddress = "tcp://<Dirección del servidor de negociación>",
	AppId = "<Identificador de aplicación>",
	AuthCode = "<Código de autenticación>".ToSecureString(),
	ProductionMode = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

