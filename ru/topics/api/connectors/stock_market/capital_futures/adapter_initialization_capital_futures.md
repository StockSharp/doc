# Инициализация адаптера: Capital Futures

В следующем коде показано, как инициализировать [CapitalFuturesMessageAdapter](xref:StockSharp.CapitalFutures.CapitalFuturesMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CapitalFuturesMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<значение>".ToSecureString(),
	SdkPath = "<значение>",
	Login = "<значение>",
	Account = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
