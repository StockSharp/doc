# Инициализация адаптера Upstox

В следующем коде показано, как инициализировать [UpstoxMessageAdapter](xref:StockSharp.Upstox.UpstoxMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new UpstoxMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Токен>".ToSecureString(),
	IsDemo = true,
	DefaultProduct = UpstoxProducts.Delivery,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
