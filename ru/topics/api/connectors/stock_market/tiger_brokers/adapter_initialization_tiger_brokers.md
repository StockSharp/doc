# Инициализация адаптера Tiger Brokers

В следующем коде показано, как инициализировать [TigerBrokersMessageAdapter](xref:StockSharp.TigerBrokers.TigerBrokersMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TigerBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	TigerId = "<Идентификатор Tiger>",
	Account = "<Счёт>",
	License = TigerLicenses.Singapore,
	PrivateKey = "<Закрытый ключ>".ToSecureString(),
	Token = "<Токен>".ToSecureString(),
	AutoGrabPermission = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

