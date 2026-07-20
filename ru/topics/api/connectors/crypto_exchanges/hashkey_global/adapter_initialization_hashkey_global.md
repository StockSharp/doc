# Инициализация адаптера HashKey Global

Код ниже демонстрирует как инициализировать [HashKeyMessageAdapter](xref:StockSharp.HashKey.HashKeyMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new HashKeyMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ваше значение>".To<SecureString>(),
	Secret = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
