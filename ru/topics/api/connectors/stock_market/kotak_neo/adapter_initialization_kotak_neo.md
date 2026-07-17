# Инициализация адаптера Kotak Neo

В следующем коде показано, как инициализировать [KotakNeoMessageAdapter](xref:StockSharp.KotakNeo.KotakNeoMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KotakNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerKey = "<Ключ потребителя>".ToSecureString(),
	MobileNumber = "<Номер мобильного телефона>",
	UserCode = "<Код пользователя>",
	Mpin = "<MPIN>".ToSecureString(),
	TotpSecret = "<Секрет TOTP>".ToSecureString(),
	DefaultProduct = KotakNeoProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
