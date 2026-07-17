# Inicialización del adaptador: Capital.com

El código siguiente muestra cómo inicializar [CapitalComMessageAdapter](xref:StockSharp.CapitalCom.CapitalComMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CapitalComMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	ApiKey = "<valor>",
	Login = "<valor>",
	AccountId = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
