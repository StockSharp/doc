# Инициализация адаптера FinViz

Код ниже демонстрирует как инициализировать [FinVizMessageAdapter](xref:StockSharp.FinViz.FinVizMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinVizMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
