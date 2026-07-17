# Инициализация адаптера Public.com

В следующем коде показано, как инициализировать [PublicMessageAdapter](xref:StockSharp.Public.PublicMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new PublicMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Токен>".ToSecureString(),
	PollingInterval = TimeSpan.FromSeconds(2),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
