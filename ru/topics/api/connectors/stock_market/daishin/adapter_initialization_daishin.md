# Инициализация адаптера: Daishin CYBOS Plus

В следующем коде показано, как инициализировать [DaishinMessageAdapter](xref:StockSharp.Daishin.DaishinMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DaishinMessageAdapter(Connector.TransactionIdGenerator)
{
	Account = "<значение>",
	IsTradingEnabled = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
