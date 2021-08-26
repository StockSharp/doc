# Инициализация адаптера Sterling

Код ниже демонстрирует как инициализировать [SterlingMessageAdapter](../api/StockSharp.Sterling.SterlingMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new SterlingMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
