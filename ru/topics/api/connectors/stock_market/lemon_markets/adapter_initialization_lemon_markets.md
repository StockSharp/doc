# Инициализация адаптера: lemon.markets

В следующем коде показано, как инициализировать [LemonMarketsMessageAdapter](xref:StockSharp.LemonMarkets.LemonMarketsMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LemonMarketsMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<значение>".ToSecureString(),
	AccountId = "<значение>",
	SecuritiesAccountId = "<значение>",
	DataPrivacyPrincipal = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
