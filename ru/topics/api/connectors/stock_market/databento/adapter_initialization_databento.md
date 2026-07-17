# Инициализация адаптера: Databento

В следующем коде показано, как инициализировать [DatabentoMessageAdapter](xref:StockSharp.Databento.DatabentoMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DatabentoMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<значение>".ToSecureString(),
	Dataset = "<значение>",
	LiveAddress = "<значение>",
	HistoricalAddress = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
