# Инициализация адаптера GRVT

Код ниже демонстрирует как инициализировать [GrvtMessageAdapter](xref:StockSharp.Grvt.GrvtMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GrvtMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ваше значение>".To<SecureString>(),
	Secret = "<Ваше значение>".To<SecureString>(),
	SubAccountId = "<Ваше значение>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
