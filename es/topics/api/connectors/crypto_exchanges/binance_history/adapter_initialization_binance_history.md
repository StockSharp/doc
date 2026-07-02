# Inicialización del adaptador Binance History

El código a continuación muestra cómo inicializar [BinanceHistoryMessageAdapter](xref:StockSharp.BinanceHistory.BinanceHistoryMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BinanceHistoryMessageAdapter(Connector.TransactionIdGenerator)
{
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
