# Инициализация адаптера Kalshi

Код ниже демонстрирует как инициализировать [KalshiMessageAdapter](xref:StockSharp.Kalshi.KalshiMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new KalshiMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ваше значение>",
	PrivateKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
