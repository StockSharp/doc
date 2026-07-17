# Инициализация адаптера: 5paisa Xstream

В следующем коде показано, как инициализировать [FivePaisaMessageAdapter](xref:StockSharp.FivePaisa.FivePaisaMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FivePaisaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<значение>".ToSecureString(),
	AppKey = "<значение>",
	ClientCode = "<значение>",
	AlgoId = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
