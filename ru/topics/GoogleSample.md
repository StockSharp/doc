# Инициализация адаптера Google

Код ниже демонстрирует как инициализировать [GoogleMessageAdapter](xref:StockSharp.Google.GoogleMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GoogleMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
