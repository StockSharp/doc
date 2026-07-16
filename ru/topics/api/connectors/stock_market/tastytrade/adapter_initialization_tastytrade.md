# Инициализация адаптера tastytrade

В следующем коде показано, как инициализировать [TastyTradeMessageAdapter](xref:StockSharp.TastyTrade.TastyTradeMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TastyTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Токен>".ToSecureString(),
	ClientSecret = "<Секрет клиента>".ToSecureString(),
	Scopes = TastyTradeScopes.Read | TastyTradeScopes.Trade,
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

