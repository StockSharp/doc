# Inicialización del adaptador: Korea Investment & Securities

El código siguiente muestra cómo inicializar [KoreaInvestmentMessageAdapter](xref:StockSharp.KoreaInvestment.KoreaInvestmentMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KoreaInvestmentMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<valor>".ToSecureString(),
	AppSecret = "<valor>".ToSecureString(),
	AccountNumber = "<valor>",
	ProductCode = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
