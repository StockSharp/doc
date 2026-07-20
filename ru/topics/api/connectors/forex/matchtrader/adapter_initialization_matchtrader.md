# Инициализация адаптера Match-Trader

Код ниже демонстрирует, как инициализировать [MatchTraderMessageAdapter](xref:StockSharp.MatchTrader.MatchTraderMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MatchTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Ваше значение>",
	Password = "<Ваше значение>".To<SecureString>(),
	AccountId = "<Ваше значение>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
