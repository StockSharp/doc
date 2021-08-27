# Инициализация адаптера UX

Код ниже демонстрирует как инициализировать [UkrExhMessageAdapter](xref:StockSharp.UkrExh.UkrExhMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new UkrExhMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
