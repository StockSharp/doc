# Инициализация адаптера OptionMetrics IvyDB

Код ниже демонстрирует как инициализировать [OptionMetricsMessageAdapter](xref:StockSharp.OptionMetrics.OptionMetricsMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OptionMetricsMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
