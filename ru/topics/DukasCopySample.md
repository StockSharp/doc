# Инициализация адаптера DukasCopy

Код ниже демонстрирует как инициализировать [DukasCopyMessageAdapter](../api/StockSharp.DukasCopy.DukasCopyMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter = new DukasCopyMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
