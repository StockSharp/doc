# Инициализация адаптера: Mirae Asset Sharekhan

В следующем коде показано, как инициализировать [MiraeSharekhanMessageAdapter](xref:StockSharp.MiraeSharekhan.MiraeSharekhanMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MiraeSharekhanMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<значение>".ToSecureString(),
	ApiKey = "<значение>",
	VendorKey = "<значение>",
	CustomerId = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
