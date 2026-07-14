# Inicialización del adaptador Webull

El siguiente código muestra cómo inicializar [WebullMessageAdapter](xref:StockSharp.Webull.WebullMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new WebullMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<clave de la aplicación>".ToSecureString(),
	Secret = "<secreto de la aplicación>".ToSecureString(),
	Token = "<token de acceso>".ToSecureString(),
	Account = "<identificador de la cuenta>",
	IsDemo = false,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Los parámetros `Token` y `Account` pueden omitirse si no son necesarios.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
