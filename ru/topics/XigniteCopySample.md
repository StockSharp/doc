# Инициализация адаптера Xignite

Код ниже демонстрирует как инициализировать [XigniteMessageAdapter](../api/StockSharp.Xignite.XigniteMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter \= new XigniteMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
