# Инициализация адаптера: Motilal Oswal

В следующем коде показано, как инициализировать [MotilalOswalMessageAdapter](xref:StockSharp.MotilalOswal.MotilalOswalMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MotilalOswalMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<значение>".ToSecureString(),
	Secret = "<значение>".ToSecureString(),
	Token = "<значение>".ToSecureString(),
	AccessToken = "<значение>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
