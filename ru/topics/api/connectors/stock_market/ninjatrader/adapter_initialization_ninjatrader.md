# Инициализация адаптера NinjaTrader

В следующем коде показано, как инициализировать [NinjaTraderMessageAdapter](xref:StockSharp.NinjaTrader.NinjaTraderMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new NinjaTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Имя пользователя>",
	Password = "<Пароль>".ToSecureString(),
	ClientId = "<Идентификатор клиента>",
	Secret = "<Секрет>".ToSecureString(),
	AppId = "StockSharp",
	AppVersion = "1.0",
	DeviceId = Guid.NewGuid().ToString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
