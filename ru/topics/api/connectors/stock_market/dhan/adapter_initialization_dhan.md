# Инициализация адаптера DhanHQ

В следующем коде показано, как инициализировать [DhanMessageAdapter](xref:StockSharp.Dhan.DhanMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DhanMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Идентификатор клиента>",
	Token = "<Токен>".ToSecureString(),
	DefaultProduct = DhanProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

