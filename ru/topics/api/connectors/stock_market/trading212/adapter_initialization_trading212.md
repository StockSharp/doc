# Инициализация адаптера: Торговля 212

В следующем коде показано, как инициализировать [Trading212MessageAdapter](xref:StockSharp.Trading212.Trading212MessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new Trading212MessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<значение>".ToSecureString(),
	ApiSecret = "<значение>".ToSecureString(),
	IsDemo = true,
	PollingInterval = TimeSpan.FromSeconds(10),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
