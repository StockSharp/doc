# Инициализация адаптера BMLL

Код ниже демонстрирует как инициализировать [BmllMessageAdapter](xref:StockSharp.Bmll.BmllMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BmllMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Ваше значение>",
	Password = "<Ваше значение>".To<SecureString>(),
	Token = "<Ваше значение>".To<SecureString>(),
	ApiKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
