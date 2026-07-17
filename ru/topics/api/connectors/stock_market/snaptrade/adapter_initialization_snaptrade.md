# Инициализация адаптера: SnapTrade

В следующем коде показано, как инициализировать [SnapTradeMessageAdapter](xref:StockSharp.SnapTrade.SnapTradeMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SnapTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerKey = "<значение>".ToSecureString(),
	UserSecret = "<значение>".ToSecureString(),
	ClientId = "<значение>",
	UserId = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
