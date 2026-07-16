# Инициализация адаптера Robinhood

В следующем коде показано, как инициализировать [RobinhoodMessageAdapter](xref:StockSharp.Robinhood.RobinhoodMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new RobinhoodMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Токен>".ToSecureString(),
	Address = new Uri("https://agent.robinhood.com/mcp/trading"),
	PollingInterval = TimeSpan.FromSeconds(2),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

