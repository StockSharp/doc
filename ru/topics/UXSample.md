# Инициализация адаптера UX

Код ниже демонстрирует как инициализировать [UkrExhMessageAdapter](../api/StockSharp.UkrExh.UkrExhMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new UkrExhMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
