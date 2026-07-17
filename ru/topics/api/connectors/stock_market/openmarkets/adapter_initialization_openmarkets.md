# Инициализация адаптера: OpenMarkets

В следующем коде показано, как инициализировать [OpenMarketsMessageAdapter](xref:StockSharp.OpenMarkets.OpenMarketsMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new OpenMarketsMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientSecret = "<значение>".ToSecureString(),
	ClientId = "<значение>",
	AccountCode = "<значение>",
	DataSource = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
