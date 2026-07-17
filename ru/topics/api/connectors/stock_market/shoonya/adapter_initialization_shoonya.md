# Инициализация адаптера: Shoonya

В следующем коде показано, как инициализировать [ShoonyaMessageAdapter](xref:StockSharp.Shoonya.ShoonyaMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ShoonyaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<значение>".ToSecureString(),
	UserId = "<значение>",
	AccountId = "<значение>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
