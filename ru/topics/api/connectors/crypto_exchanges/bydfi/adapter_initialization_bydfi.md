# Инициализация адаптера BYDFi

Код ниже демонстрирует как инициализировать [BYDFiMessageAdapter](xref:StockSharp.BYDFi.BYDFiMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BYDFiMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ваше значение>".To<SecureString>(),
	Secret = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
