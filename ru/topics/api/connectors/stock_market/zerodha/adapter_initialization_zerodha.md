# Инициализация адаптера: Zerodha Kite Connect

В следующем коде показано, как инициализировать [ZerodhaMessageAdapter](xref:StockSharp.Zerodha.ZerodhaMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ZerodhaMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiSecret = "<значение>".ToSecureString(),
	Token = "<значение>".ToSecureString(),
	RequestToken = "<значение>".ToSecureString(),
	ApiKey = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
