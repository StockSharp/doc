# Инициализация адаптера TradeLocker

Код ниже демонстрирует как инициализировать [TradeLockerMessageAdapter](xref:StockSharp.TradeLocker.TradeLockerMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TradeLockerMessageAdapter(Connector.TransactionIdGenerator)
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
