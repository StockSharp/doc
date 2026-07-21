# Inicialización del adaptador GMTrade

El siguiente código muestra cómo inicializar [GMTradeMessageAdapter](xref:StockSharp.GMTrade.GMTradeMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GMTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Su valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
