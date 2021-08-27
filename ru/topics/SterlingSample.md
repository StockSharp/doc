# Инициализация адаптера Sterling

Код ниже демонстрирует как инициализировать [SterlingMessageAdapter](xref:StockSharp.Sterling.SterlingMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new SterlingMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
