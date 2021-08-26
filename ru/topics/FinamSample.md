# Инициализация адаптера Finam

Код ниже демонстрирует как инициализировать [FinamMessageAdapter](../api/StockSharp.Finam.FinamMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter = new AlorHistoryMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
