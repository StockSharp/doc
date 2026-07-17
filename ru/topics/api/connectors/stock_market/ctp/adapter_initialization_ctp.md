# Инициализация адаптера CTP

В следующем коде показано, как инициализировать [CtpMessageAdapter](xref:StockSharp.Ctp.CtpMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CtpMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Имя пользователя>",
	Password = "<Пароль>".ToSecureString(),
	BrokerId = "<Идентификатор брокера>",
	InvestorId = "<Идентификатор инвестора>",
	MarketDataAddress = "tcp://<Адрес сервера рыночных данных>",
	TraderAddress = "tcp://<Адрес торгового сервера>",
	AppId = "<Идентификатор приложения>",
	AuthCode = "<Код аутентификации>".ToSecureString(),
	ProductionMode = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
