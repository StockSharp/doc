# Инициализация адаптера: kabu Station

В следующем коде показано, как инициализировать [KabuStationMessageAdapter](xref:StockSharp.KabuStation.KabuStationMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KabuStationMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiPassword = "<значение>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
