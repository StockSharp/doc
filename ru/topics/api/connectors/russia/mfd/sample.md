# Инициализация адаптера Mfd

Код ниже демонстрирует как инициализировать [MfdMessageAdapter](xref:StockSharp.Mfd.MfdMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MfdMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
