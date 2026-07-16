# Inicialización del adaptador IG Markets

El código siguiente muestra cómo inicializar [IgMessageAdapter](xref:StockSharp.IG.IgMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new IgMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Clave de API>",
	UserName = "<Nombre de usuario>",
	Password = "<Contraseña>".ToSecureString(),
	AccountId = "<Identificador de cuenta>",
	Environment = IgEnvironments.Demo,
	EncryptPassword = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

