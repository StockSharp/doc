# Инициализация адаптера Angel One

В следующем коде показано, как инициализировать [AngelOneMessageAdapter](xref:StockSharp.AngelOne.AngelOneMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new AngelOneMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Имя пользователя>",
	Password = "<Пароль>".ToSecureString(),
	ApiKey = "<Ключ API>".ToSecureString(),
	TotpSecret = "<Секрет TOTP>".ToSecureString(),
	ClientLocalIp = "127.0.0.1",
	ClientPublicIp = "<Публичный IP-адрес клиента>",
	MacAddress = "<MAC-адрес>",
	DefaultProduct = AngelOneProducts.Delivery,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

