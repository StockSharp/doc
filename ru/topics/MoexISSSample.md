# Инициализация адаптера MOEX ISS

Код ниже демонстрирует как инициализировать [MoexISSMessageAdapter](xref:StockSharp.MoexISS.MoexISSMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MoexISSMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
