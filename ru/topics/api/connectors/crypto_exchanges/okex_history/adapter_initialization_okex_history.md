# Инициализация адаптера OKEx History

Код ниже демонстрирует, как инициализировать [OkexHistoryMessageAdapter](xref:StockSharp.OkexHistory.OkexHistoryMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OkexHistoryMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
