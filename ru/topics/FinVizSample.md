# Инициализация адаптера FinViz

Код ниже демонстрирует как инициализировать [FinVizMessageAdapter](../api/StockSharp.FinViz.FinVizMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter = new FinVizMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
