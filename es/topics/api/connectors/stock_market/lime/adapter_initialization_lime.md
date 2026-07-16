# Inicialización del adaptador Lime

El código siguiente muestra cómo inicializar [LimeMessageAdapter](xref:StockSharp.Lime.LimeMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LimeMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Nombre de usuario>",
	Password = "<Contraseña>".ToSecureString(),
	ClientId = "<Identificador de cliente>",
	ClientSecret = "<Secreto del cliente>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
