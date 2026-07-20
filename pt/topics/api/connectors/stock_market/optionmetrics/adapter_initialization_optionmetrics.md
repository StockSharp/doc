# Inicialização do adaptador OptionMetrics IvyDB

O código abaixo demonstra como inicializar o [OptionMetricsMessageAdapter](xref:StockSharp.OptionMetrics.OptionMetricsMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OptionMetricsMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
