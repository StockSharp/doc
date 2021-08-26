# Инициализация адаптера Yahoo

Код ниже демонстрирует как инициализировать [YahooMessageAdapter](../api/StockSharp.Yahoo.YahooMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter = new YahooMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
