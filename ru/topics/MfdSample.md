# Инициализация адаптера Mfd

Код ниже демонстрирует как инициализировать [MfdMessageAdapter](../api/StockSharp.Mfd.MfdMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter = new MfdMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
