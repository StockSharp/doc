# Inicialización del adaptador Saxo OpenAPI

El código siguiente muestra cómo inicializar [SaxoMessageAdapter](xref:StockSharp.Saxo.SaxoMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SaxoMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<Token de acceso>".ToSecureString(),
	RefreshToken = "<Token de actualización>".ToSecureString(),
	ClientId = "<Identificador de cliente>",
	ClientSecret = "<Secreto del cliente>".ToSecureString(),
	RedirectUri = "<URI de redirección>",
	AccountKey = "<Clave de cuenta>",
	Environment = SaxoEnvironments.Simulation,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
