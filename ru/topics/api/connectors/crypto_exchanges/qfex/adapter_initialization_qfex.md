# Инициализация адаптера QFEX

Код ниже демонстрирует как инициализировать [QFEXMessageAdapter](xref:StockSharp.QFEX.QFEXMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new QFEXMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ваше значение>",
	Secret = "<Ваше значение>".To<SecureString>(),
	AccountId = "<Ваше значение>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
