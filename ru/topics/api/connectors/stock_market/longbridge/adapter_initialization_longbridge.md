# Инициализация адаптера Longbridge OpenAPI

В следующем коде показано, как инициализировать [LongbridgeMessageAdapter](xref:StockSharp.Longbridge.LongbridgeMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LongbridgeMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<Ключ приложения>",
	AppSecret = "<Секрет приложения>".ToSecureString(),
	AccessToken = "<Токен доступа>".ToSecureString(),
	Portfolio = "Longbridge",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

