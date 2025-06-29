# Инициализация адаптера Finam

Код ниже демонстрирует как инициализировать [FinamMessageAdapter](xref:StockSharp.Finam.FinamMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinamMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
