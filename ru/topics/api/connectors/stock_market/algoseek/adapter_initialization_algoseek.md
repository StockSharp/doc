# Инициализация адаптера AlgoSeek

Код ниже демонстрирует как инициализировать [AlgoSeekMessageAdapter](xref:StockSharp.AlgoSeek.AlgoSeekMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AlgoSeekMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
