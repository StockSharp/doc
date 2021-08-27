# Инициализация адаптера GAIN Capital

Код ниже демонстрирует как инициализировать [GainCapitalMessageAdapter](xref:StockSharp.GainCapital.GainCapitalMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GainCapitalMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
