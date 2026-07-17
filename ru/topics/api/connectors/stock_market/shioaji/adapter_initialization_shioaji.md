# Инициализация адаптера: SinoPac Shioaji

В следующем коде показано, как инициализировать [ShioajiMessageAdapter](xref:StockSharp.Shioaji.ShioajiMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ShioajiMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<значение>".ToSecureString(),
	Secret = "<значение>".ToSecureString(),
	Address = "<значение>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
