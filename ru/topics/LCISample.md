# Инициализация адаптера ЛЧИ

Код ниже демонстрирует как инициализировать [MoexLchiMessageAdapter](../api/StockSharp.MoexLchi.MoexLchiMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new MoexLchiMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
