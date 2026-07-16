# Инициализация адаптера Lime Trader

В следующем коде показано, как инициализировать [LimeMessageAdapter](xref:StockSharp.Lime.LimeMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LimeMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Имя пользователя>",
	Password = "<Пароль>".ToSecureString(),
	ClientId = "<Идентификатор клиента>",
	ClientSecret = "<Секрет клиента>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

