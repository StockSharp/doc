# Инициализация адаптера CQG Web API

В следующем коде показано, как инициализировать [CqgMessageAdapter](xref:StockSharp.CQG.CqgMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CqgMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Имя пользователя>",
	Password = "<Пароль>".ToSecureString(),
	PrivateLabel = "WebAPITest",
	ClientId = "WebAPITest",
	Endpoint = "wss://demoapi.cqg.com:443",
	Portfolio = "<Портфель>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
