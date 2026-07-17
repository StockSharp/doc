# Инициализация адаптера: Sierra Chart DTC

В следующем коде показано, как инициализировать [SierraChartDtcMessageAdapter](xref:StockSharp.SierraChartDtc.SierraChartDtcMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SierraChartDtcMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<значение>".ToSecureString(),
	Login = "<значение>",
	TradeAccount = "<значение>",
	TargetHost = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
