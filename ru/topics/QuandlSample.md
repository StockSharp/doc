# Инициализация адаптера Quandl

Код ниже демонстрирует как инициализировать [QuandlMessageAdapter](xref:StockSharp.Quandl.QuandlMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QuandlMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
