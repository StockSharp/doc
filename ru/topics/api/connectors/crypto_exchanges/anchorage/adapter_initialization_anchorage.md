# Инициализация адаптера Anchorage

Код ниже демонстрирует как инициализировать [AnchorageMessageAdapter](xref:StockSharp.Anchorage.AnchorageMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AnchorageMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ваше значение>".To<SecureString>(),
	SigningKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
