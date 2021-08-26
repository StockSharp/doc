# Инициализация адаптера GAIN Capital

Код ниже демонстрирует как инициализировать [GainCapitalMessageAdapter](../api/StockSharp.GainCapital.GainCapitalMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter \= new GainCapitalMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
