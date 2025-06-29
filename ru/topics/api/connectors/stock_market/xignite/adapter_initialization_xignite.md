# Инициализация адаптера Xignite

Код ниже демонстрирует, как инициализировать [XigniteMessageAdapter](xref:StockSharp.Xignite.XigniteMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new XigniteMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
