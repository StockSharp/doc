# Инициализация адаптера NDAX

Код ниже демонстрирует как инициализировать [NDAXMessageAdapter](xref:StockSharp.NDAX.NDAXMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new NDAXMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ваше значение>".To<SecureString>(),
	Secret = "<Ваше значение>".To<SecureString>(),
	UserId = 1,
	AccountId = 1,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
