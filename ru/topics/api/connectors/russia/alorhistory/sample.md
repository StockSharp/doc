# Инициализация адаптера AlorHistory

Код ниже демонстрирует как инициализировать [AlorHistoryMessageAdapter](xref:StockSharp.AlorHistory.AlorHistoryMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new AlorHistoryMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
