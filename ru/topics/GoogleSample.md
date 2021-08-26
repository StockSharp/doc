# Инициализация адаптера Google

Код ниже демонстрирует как инициализировать [GoogleMessageAdapter](../api/StockSharp.Google.GoogleMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter \= new GoogleMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
