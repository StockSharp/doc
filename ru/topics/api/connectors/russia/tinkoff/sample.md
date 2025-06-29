# Инициализация адаптера Tinkoff

Код ниже демонстрирует как инициализировать [TinkoffMessageAdapter](xref:StockSharp.Tinkoff.TinkoffMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new TinkoffMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your Token>",
        // раскомментировать для подключения к демо торгам
	//IsDemo = true,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
