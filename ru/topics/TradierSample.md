# Инициализация адаптера Tradier

Код ниже демонстрирует как инициализировать [TradierMessageAdapter](../api/StockSharp.Tradier.TradierMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter \= new TradierMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
