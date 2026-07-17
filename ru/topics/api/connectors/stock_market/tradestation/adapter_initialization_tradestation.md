# Инициализация адаптера TradeStation

В следующем коде показано, как инициализировать [TradeStationMessageAdapter](xref:StockSharp.TradeStation.TradeStationMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradeStationMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Токен>".ToSecureString(),
	IsDemo = true,
	DefaultRoute = "Intelligent",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
