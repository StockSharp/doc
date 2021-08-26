# Инициализация адаптера Quandl

Код ниже демонстрирует как инициализировать [QuandlMessageAdapter](../api/StockSharp.Quandl.QuandlMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter \= new QuandlMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
