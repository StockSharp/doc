# Inicialización del adaptador ACTIV Financial

El siguiente código muestra cómo inicializar [ActivFinancialMessageAdapter](xref:StockSharp.ActivFinancial.ActivFinancialMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ActivFinancialMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Su valor>",
	Password = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
