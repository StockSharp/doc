# Инициализация адаптера Tradier

Код ниже демонстрирует как инициализировать [TradierMessageAdapter](xref:StockSharp.Tradier.TradierMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradierMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
