# Инициализация адаптера DukasCopy

Код ниже демонстрирует как инициализировать [DukasCopyMessageAdapter](xref:StockSharp.DukasCopy.DukasCopyMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DukasCopyMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
