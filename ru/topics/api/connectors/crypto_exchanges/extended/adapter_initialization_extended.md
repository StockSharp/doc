# Инициализация адаптера Extended

Код ниже демонстрирует как инициализировать [ExtendedMessageAdapter](xref:StockSharp.Extended.ExtendedMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ExtendedMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ваше значение>".To<SecureString>(),
	PrivateKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
