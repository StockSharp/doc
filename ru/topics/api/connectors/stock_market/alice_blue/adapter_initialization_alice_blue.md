# Инициализация адаптера: Alice Blue

В следующем коде показано, как инициализировать [AliceBlueMessageAdapter](xref:StockSharp.AliceBlue.AliceBlueMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new AliceBlueMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<значение>".ToSecureString(),
	UserId = "<значение>",
	ClientId = "<значение>",
	DeviceId = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
