# Инициализация адаптера E\*TRADE

Код ниже демонстрирует, как инициализировать [ETradeMessageAdapter](xref:StockSharp.ETrade.ETradeMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new ETradeMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerSecret = "<Ваш секрет>".To<SecureString>(),
	ConsumerKey = "<Ваш ключ>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
