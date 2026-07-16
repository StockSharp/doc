# Инициализация адаптера FYERS

В следующем коде показано, как инициализировать [FyersMessageAdapter](xref:StockSharp.Fyers.FyersMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FyersMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Идентификатор клиента>",
	Token = "<Токен>".ToSecureString(),
	DefaultProduct = FyersProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

