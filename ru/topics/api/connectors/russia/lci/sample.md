# Инициализация адаптера ЛЧИ

Код ниже демонстрирует как инициализировать [MoexLchiMessageAdapter](xref:StockSharp.MoexLchi.MoexLchiMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new MoexLchiMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
