# Инициализация адаптера Moomoo

В следующем коде показано, как инициализировать [MoomooMessageAdapter](xref:StockSharp.Moomoo.MoomooMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MoomooMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Loopback, 11111),
	Password = "<Пароль>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
