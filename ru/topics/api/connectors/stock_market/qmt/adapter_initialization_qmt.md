# Инициализация адаптера: QMT

В следующем коде показано, как инициализировать [QmtMessageAdapter](xref:StockSharp.Qmt.QmtMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QmtMessageAdapter(Connector.TransactionIdGenerator)
{
	GatewayToken = "<значение>".ToSecureString(),
	GatewayHost = "<значение>",
	GatewayPort = 10,
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
