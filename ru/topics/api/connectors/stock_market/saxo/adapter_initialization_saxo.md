# Инициализация адаптера Saxo OpenAPI

В следующем коде показано, как инициализировать [SaxoMessageAdapter](xref:StockSharp.Saxo.SaxoMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SaxoMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<Токен доступа>".ToSecureString(),
	RefreshToken = "<Токен обновления>".ToSecureString(),
	ClientId = "<Идентификатор клиента>",
	ClientSecret = "<Секрет клиента>".ToSecureString(),
	RedirectUri = "<URI перенаправления>",
	AccountKey = "<Ключ счёта>",
	Environment = SaxoEnvironments.Simulation,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
