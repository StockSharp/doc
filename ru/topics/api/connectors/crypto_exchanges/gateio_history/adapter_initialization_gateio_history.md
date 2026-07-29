# Инициализация адаптера истории Gate.io

Код ниже показывает, как инициализировать [GateIOHistoryMessageAdapter](xref:StockSharp.GateIOHistory.GateIOHistoryMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new();
...
var messageAdapter = new GateIOHistoryMessageAdapter(connector.TransactionIdGenerator)
{
	CheckDates = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
