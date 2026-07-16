# Инициализация адаптера IG Markets

В следующем коде показано, как инициализировать [IgMessageAdapter](xref:StockSharp.IG.IgMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new IgMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ключ API>",
	UserName = "<Имя пользователя>",
	Password = "<Пароль>".ToSecureString(),
	AccountId = "<Идентификатор счёта>",
	Environment = IgEnvironments.Demo,
	EncryptPassword = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

