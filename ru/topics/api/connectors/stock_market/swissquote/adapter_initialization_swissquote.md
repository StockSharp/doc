# Инициализация адаптера: Swissquote OpenWealth

В следующем коде показано, как инициализировать [SwissquoteMessageAdapter](xref:StockSharp.Swissquote.SwissquoteMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SwissquoteMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<значение>".ToSecureString(),
	CustomerId = "<значение>",
	SafekeepingAccountId = "<значение>",
	CashAccountId = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
