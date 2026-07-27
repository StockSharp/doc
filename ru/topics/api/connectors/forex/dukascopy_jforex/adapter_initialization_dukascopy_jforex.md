# Инициализация адаптера: DukasCopy JForex

В следующем коде показано, как инициализировать [DukasCopyJForexMessageAdapter](xref:StockSharp.DukasCopyJForex.DukasCopyJForexMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DukasCopyJForexMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<значение>".ToSecureString(),
	Login = "<значение>",
	BridgeJarPath = "<значение>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
