# Инициализация адаптера ICICI Direct Breeze

В следующем коде показано, как инициализировать [BreezeMessageAdapter](xref:StockSharp.Breeze.BreezeMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BreezeMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ключ API>",
	SecretKey = "<Секретный ключ>".ToSecureString(),
	ApiSession = "<Сессия API>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
